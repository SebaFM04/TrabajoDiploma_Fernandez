; ============================================================
;  Instalador TD 2026 - Sebastian Fernandez
;  .NET Framework 4.8 + SQL Server Express (si falta) + base de datos
;
;  Uso normal:       doble clic en el Setup
;  Uso desatendido:  Setup.exe /VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LOG="instalacion.log"
;  Parametros opcionales:
;     /INSTANCIA=".\SQLEXPRESS"   fuerza otra instancia de SQL Server (por defecto usa ".")
;     /PISARBD=si                 si la base ya existe, la borra y la crea de nuevo
;
;  Que resuelve solo:
;     - Falta .NET Framework 4.8      -> lo instala
;     - Falta SQL Server              -> instala SQL Server Express como instancia por defecto (".")
;     - Instancia: no se pregunta. Usa "." si existe; si la PC solo tiene instancias con nombre
;       (ej: .\SQLEXPRESS), usa la primera que encuentra.
;     - Servicio de SQL detenido      -> lo pone en automatico y lo inicia
;     - Falta sqlcmd en la PC         -> usa uno propio (go-sqlcmd) incluido en el setup
;     - Nunca pide reinicio (PCs congeladas): todo tiene que funcionar en la misma sesion
;     - Permisos en SQL: crea la base con el usuario que tenga permisos, en este orden:
;         1) el usuario elevado (el que autorizo la instalacion, ej. Tecnico)
;         2) el usuario que abrio el setup (ej. Navegador)
;         3) si ninguno puede, reinicia SQL en modo usuario unico y le da permisos al usuario elevado
; ============================================================

; ---------- CAMBIAR ESTOS VALORES ----------
#define MyAppName        "SistemaBar"
#define MyAppVersion     "1.0"
#define MyAppPublisher   "Sebastian Fernandez"
#define MyAppExeName     "SistemaBar.exe"
#define MyDbName         "TpIngSoftware_2026"
#define ReleaseDir       "..\UI\bin\Release"
; Relativos a la carpeta donde esta este .iss
#define SqlScript        "script.sql"
#define NetInstaller     "redist\ndp48-x86-x64-allos-enu.exe"
#define SqlExpress       "redist\SQLEXPR_x64_ENU.exe"
#define GoSqlcmd         "redist\sqlcmd.exe"
; -------------------------------------------

[Setup]
AppId={{8D3F2A61-4C7B-4E9A-B15D-6A2E9F0C7D34}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\{#MyAppExeName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
OutputDir=Output
OutputBaseFilename=Setup_{#MyAppName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
SetupLogging=yes
CloseApplications=yes
; PCs congeladas (Deep Freeze): nunca pedir reinicio, se perderia todo lo instalado
RestartIfNeededByRun=no

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Todo lo compilado en Release (exe, dlls, .config). No se copian .pdb ni el conexion.txt de tu PC.
Source: "{#ReleaseDir}\*"; DestDir: "{app}"; Excludes: "*.pdb,*.vshost.exe*,conexion.txt"; Flags: ignoreversion recursesubdirs createallsubdirs
; Script de la base y sqlcmd: van a {app}\instalacion porque tambien los tiene que poder usar
; el usuario comun (la carpeta temporal del instalador solo la leen administradores). Se borran al terminar.
Source: "{#SqlScript}"; DestDir: "{app}\instalacion"; Flags: ignoreversion
Source: "{#GoSqlcmd}"; DestDir: "{app}\instalacion"; Flags: ignoreversion
; Instaladores y herramientas: solo se extraen si hacen falta (nocompression: ya vienen comprimidos)
Source: "{#NetInstaller}"; Flags: dontcopy nocompression
Source: "{#SqlExpress}"; Flags: dontcopy nocompression

[Dirs]
; Los usuarios comunes pueden escribir aca (para el log de SQL cuando se usa su cuenta)
Name: "{app}\instalacion"; Permissions: users-modify

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent; Check: PuedeLanzar

[UninstallDelete]
Type: files; Name: "{app}\conexion.txt"
Type: filesandordirs; Name: "{app}\instalacion"

[Code]
var
  Instancias: TStringList;
  HaySQL: Boolean;
  SqlComoOriginal: Boolean;  { True = los comandos SQL se ejecutan como el usuario que abrio el setup }
  NecesitaReinicio: Boolean;

{ ---------- Deteccion de instancias de SQL Server ---------- }

procedure AgregarInstancias(RootKey: Integer);
var
  Nombres: TArrayOfString;
  I: Integer;
  Inst: String;
begin
  if RegGetValueNames(RootKey, 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL', Nombres) then
    for I := 0 to GetArrayLength(Nombres) - 1 do
    begin
      if CompareText(Nombres[I], 'MSSQLSERVER') = 0 then
        Inst := '.'
      else
        Inst := '.\' + Nombres[I];
      if Instancias.IndexOf(Inst) < 0 then
      begin
        if Inst = '.' then
          Instancias.Insert(0, Inst)  { la instancia por defecto siempre primero }
        else
          Instancias.Add(Inst);
      end;
    end;
end;

procedure BuscarInstanciasSQL();
begin
  Instancias := TStringList.Create;
  AgregarInstancias(HKLM64);
  AgregarInstancias(HKLM32);
  HaySQL := Instancias.Count > 0;
  if HaySQL then
    Log('SQL Server detectado: ' + IntToStr(Instancias.Count) + ' instancia(s)')
  else
  begin
    Log('No hay SQL Server instalado, se instalara SQL Server Express');
    Instancias.Add('.');
  end;
end;

function ObtenerInstancia(): String;
begin
  { Parametro /INSTANCIA si lo pasaron; si no, "." (o la primera detectada si no hay instancia por defecto) }
  Result := Trim(ExpandConstant('{param:INSTANCIA|}'));
  if Result = '' then
    Result := Instancias[0];
end;

function UsuarioActual(): String;
begin
  Result := ExpandConstant('{%USERDOMAIN}\{username}');
end;

{ ---------- Servicio de SQL Server ---------- }

function EsLocal(Servidor: String): Boolean;
var
  S: String;
begin
  S := Uppercase(Servidor);
  Result := (S = '.') or (S = '(LOCAL)') or (S = 'LOCALHOST') or (S = Uppercase(GetComputerNameString()));
end;

function NombreServicio(Instancia: String): String;
var
  P: Integer;
begin
  Result := '';
  P := Pos('\', Instancia);
  if P = 0 then
  begin
    if EsLocal(Instancia) then
      Result := 'MSSQLSERVER';
  end
  else if EsLocal(Copy(Instancia, 1, P - 1)) then
    Result := 'MSSQL$' + Copy(Instancia, P + 1, Length(Instancia));
end;

procedure IniciarServicio(Instancia: String);
var
  Servicio: String;
  RC: Integer;
begin
  Servicio := NombreServicio(Instancia);
  if Servicio = '' then
    Exit;  { servidor remoto: no se toca }
  Log('Asegurando servicio ' + Servicio);
  { Si lo deshabilitaron, lo vuelve a automatico; despues lo inicia (si ya estaba iniciado no pasa nada) }
  Exec(ExpandConstant('{sys}\sc.exe'), 'config "' + Servicio + '" start= auto', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Exec(ExpandConstant('{sys}\net.exe'), 'start "' + Servicio + '"', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Log('net start devolvio ' + IntToStr(RC) + ' (0 = iniciado, 2 = ya estaba iniciado)');
end;

{ ---------- sqlcmd propio (go-sqlcmd), no depende de lo instalado en la PC ---------- }

function RutaSqlcmd(): String;
begin
  Result := ExpandConstant('{app}\instalacion\sqlcmd.exe');
end;

function ArgsSql(Instancia: String): String;
begin
  { -E: autenticacion de Windows | -C: confiar en el certificado del servidor }
  Result := '-S "' + Instancia + '" -E -C';
end;

function EjecutarSqlcmdComo(Args: String; ComoOriginal: Boolean; var ResultCode: Integer): Boolean;
begin
  if ComoOriginal then
  begin
    Log('sqlcmd (como usuario original) ' + Args);
    Result := ExecAsOriginalUser(RutaSqlcmd(), Args, '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  end
  else
  begin
    Log('sqlcmd (como usuario elevado) ' + Args);
    Result := Exec(RutaSqlcmd(), Args, '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  end;
  if Result then
    Log('sqlcmd termino con codigo ' + IntToStr(ResultCode))
  else
    Log('No se pudo ejecutar sqlcmd');
end;

function EjecutarSqlcmd(Args: String; var ResultCode: Integer): Boolean;
begin
  Result := EjecutarSqlcmdComo(Args, SqlComoOriginal, ResultCode);
end;

function PuedeCrearBases(Instancia: String; ComoOriginal: Boolean): Boolean;
var
  RC: Integer;
begin
  { Codigo 0 = puede entrar y es sysadmin o puede crear bases }
  Result := EjecutarSqlcmdComo(ArgsSql(Instancia) + ' -b -l 15 -Q "IF IS_SRVROLEMEMBER(''sysadmin'') <> 1 ' +
    'AND HAS_PERMS_BY_NAME(NULL, NULL, ''CREATE ANY DATABASE'') <> 1 RAISERROR(''SinPermisos'', 16, 1)"',
    ComoOriginal, RC) and (RC = 0);
end;

procedure DarAcceso(Instancia, Cuenta: String);
var
  RC: Integer;
begin
  { Crea el login y le da db_owner SOLO sobre la base del sistema. Si algo ya existe, lo ignora. }
  EjecutarSqlcmd(ArgsSql(Instancia) + ' -Q "' +
    'BEGIN TRY CREATE LOGIN [' + Cuenta + '] FROM WINDOWS; END TRY BEGIN CATCH END CATCH; ' +
    'USE [{#MyDbName}]; ' +
    'BEGIN TRY CREATE USER [' + Cuenta + '] FOR LOGIN [' + Cuenta + ']; END TRY BEGIN CATCH END CATCH; ' +
    'BEGIN TRY ALTER ROLE db_owner ADD MEMBER [' + Cuenta + ']; END TRY BEGIN CATCH END CATCH;"', RC);
end;

{ Ultimo recurso (procedimiento documentado por Microsoft): con SQL en modo usuario unico,
  cualquier administrador de Windows entra como sysadmin. Se usa para darle permisos al usuario elevado. }
function PermisosPorModoUsuarioUnico(Instancia: String): Boolean;
var
  Servicio, Usuario: String;
  RC, Intento: Integer;
begin
  Result := False;
  Servicio := NombreServicio(Instancia);
  if Servicio = '' then
    Exit;
  Usuario := UsuarioActual();
  Log('Modo usuario unico: dando sysadmin a ' + Usuario + ' en ' + Servicio);

  Exec(ExpandConstant('{sys}\net.exe'), 'stop "' + Servicio + '" /y', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Exec(ExpandConstant('{sys}\net.exe'), 'start "' + Servicio + '" /m', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Log('net start /m devolvio ' + IntToStr(RC));

  { Puede tardar en aceptar conexiones, o otro servicio tomar la unica conexion: se reintenta }
  for Intento := 1 to 5 do
  begin
    if EjecutarSqlcmdComo(ArgsSql(Instancia) + ' -b -l 10 -Q "' +
         'BEGIN TRY CREATE LOGIN [' + Usuario + '] FROM WINDOWS; END TRY BEGIN CATCH END CATCH; ' +
         'ALTER SERVER ROLE sysadmin ADD MEMBER [' + Usuario + '];"', False, RC) and (RC = 0) then
    begin
      Result := True;
      Break;
    end;
    Sleep(3000);
  end;

  { Volver a modo normal siempre, haya funcionado o no }
  Exec(ExpandConstant('{sys}\net.exe'), 'stop "' + Servicio + '" /y', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Exec(ExpandConstant('{sys}\net.exe'), 'start "' + Servicio + '"', '', SW_HIDE, ewWaitUntilTerminated, RC);
  Log('Modo usuario unico terminado, resultado=' + IntToStr(Ord(Result)));
end;

{ Decide con que usuario se ejecutan los comandos SQL. False = nadie tiene permisos. }
function ElegirUsuarioSql(Instancia: String): Boolean;
begin
  Result := True;
  SqlComoOriginal := False;
  if PuedeCrearBases(Instancia, False) then
  begin
    Log('SQL: se usa el usuario elevado (' + UsuarioActual() + ')');
    Exit;
  end;
  if PuedeCrearBases(Instancia, True) then
  begin
    SqlComoOriginal := True;
    Log('SQL: el usuario elevado no tiene permisos, se usa el usuario que abrio el setup');
    Exit;
  end;
  if PermisosPorModoUsuarioUnico(Instancia) and PuedeCrearBases(Instancia, False) then
  begin
    Log('SQL: permisos obtenidos por modo usuario unico, se usa el usuario elevado');
    Exit;
  end;
  Result := False;
  Log('SQL: ningun usuario tiene permisos para crear la base');
end;

function PuedeLanzar(): Boolean;
begin
  { Aunque algun componente haya pedido reinicio, se permite abrir el programa:
    en una PC congelada reiniciar borraria la instalacion }
  Result := True;
end;

{ ---------- Inicio: detectar SQL (sin pantallas de configuracion) ---------- }

procedure InitializeWizard();
begin
  BuscarInstanciasSQL();
  Log('Instancia a usar: ' + ObtenerInstancia());
end;

{ ---------- Juntar logs de SQL en el Escritorio cuando algo falla ---------- }

procedure CopiarArchivo(Origen, Destino: String);
begin
  if FileExists(Origen) then
    if not FileCopy(Origen, AddBackslash(Destino) + ExtractFileName(Origen), False) then
      Log('No se pudo copiar ' + Origen);
end;

procedure CopiarPatron(Carpeta, Patron, Destino: String);
var
  FR: TFindRec;
begin
  if FindFirst(AddBackslash(Carpeta) + Patron, FR) then
  try
    repeat
      if (FR.Attributes and FILE_ATTRIBUTE_DIRECTORY) = 0 then
        CopiarArchivo(AddBackslash(Carpeta) + FR.Name, Destino);
    until not FindNext(FR);
  finally
    FindClose(FR);
  end;
end;

function SubcarpetaMasNueva(Carpeta: String): String;
var
  FR: TFindRec;
begin
  { Las carpetas de log de SQL se llaman AAAAMMDD_hhmmss, la mayor alfabeticamente es la mas nueva }
  Result := '';
  if FindFirst(AddBackslash(Carpeta) + '*', FR) then
  try
    repeat
      if ((FR.Attributes and FILE_ATTRIBUTE_DIRECTORY) <> 0) and (FR.Name <> '.') and (FR.Name <> '..') then
        if CompareText(FR.Name, Result) > 0 then
          Result := FR.Name;
    until not FindNext(FR);
  finally
    FindClose(FR);
  end;
end;

function GuardarLogsSQL(): String;
var
  FR: TFindRec;
  Base, Nueva: String;
begin
  Result := ExpandConstant('{userdesktop}\{#MyAppName}_logs');
  ForceDirectories(Result);

  { a) Salida de consola del setup de SQL (lo que imprime antes de poder crear sus propios logs) }
  CopiarArchivo(ExpandConstant('{tmp}\sqlsetup_consola.txt'), Result);

  { b) Logs tempranos que SQL deja en %TEMP% cuando falla antes de crear su carpeta }
  CopiarPatron(ExpandConstant('{%TEMP}'), 'SqlSetup*.log', Result);

  { c) Logs normales en Program Files\Microsoft SQL Server\<version>\Setup Bootstrap\Log (si llegaron a existir) }
  if FindFirst(ExpandConstant('{commonpf64}\Microsoft SQL Server\*'), FR) then
  try
    repeat
      if ((FR.Attributes and FILE_ATTRIBUTE_DIRECTORY) <> 0) and (FR.Name <> '.') and (FR.Name <> '..') then
      begin
        Base := ExpandConstant('{commonpf64}\Microsoft SQL Server\') + FR.Name + '\Setup Bootstrap\Log';
        CopiarArchivo(Base + '\Summary.txt', Result);
        Nueva := SubcarpetaMasNueva(Base);
        if Nueva <> '' then
        begin
          CopiarPatron(Base + '\' + Nueva, 'Summary*.txt', Result);
          CopiarPatron(Base + '\' + Nueva, 'Detail*.txt', Result);
        end;
      end;
    until not FindNext(FR);
  finally
    FindClose(FR);
  end;

  { d) Log de este instalador }
  if ExpandConstant('{log}') <> '' then
    CopiarArchivo(ExpandConstant('{log}'), Result);

  Log('Logs de SQL copiados a ' + Result);
end;

{ ---------- .NET 4.8 y SQL Server Express (antes de copiar archivos) ---------- }

function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  ResultCode: Integer;
  Progreso: TOutputProgressWizardPage;
  CarpetaSql, ParamsSql, CarpetaLogs, ParamIdioma: String;
begin
  Result := '';
  Progreso := CreateOutputProgressPage('Instalando componentes necesarios',
    'Esto puede tardar varios minutos. No cierre el instalador.');
  Progreso.ProgressBar.Style := npbstMarquee;
  Progreso.Show;
  try
    { 1. .NET Framework 4.8 }
    if IsDotNetInstalled(net48, 0) then
      Log('.NET Framework 4.8 ya instalado')
    else
    begin
      Progreso.SetText('Instalando .NET Framework 4.8...', '');
      ExtractTemporaryFile('ndp48-x86-x64-allos-enu.exe');
      if not Exec(ExpandConstant('{tmp}\ndp48-x86-x64-allos-enu.exe'), '/q /norestart', '',
                  SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
        Result := 'No se pudo ejecutar el instalador de .NET Framework 4.8.';
        Exit;
      end;
      Log('.NET 4.8 termino con codigo ' + IntToStr(ResultCode));
      if (ResultCode = 3010) or (ResultCode = 1641) then
        NecesitaReinicio := True
      else if ResultCode <> 0 then
      begin
        Result := 'Error ' + IntToStr(ResultCode) + ' al instalar .NET Framework 4.8.';
        Exit;
      end;
    end;

    { 2. SQL Server Express, solo si no habia ninguna instancia }
    if not HaySQL then
    begin
      Progreso.SetText('Instalando SQL Server Express...', 'Puede tardar 10 minutos o más.');
      ExtractTemporaryFile('SQLEXPR_x64_ENU.exe');
      CarpetaSql := ExpandConstant('{tmp}\sqlsetup');

      { Descomprimir el paquete }
      if not Exec(ExpandConstant('{tmp}\SQLEXPR_x64_ENU.exe'), '/x:"' + CarpetaSql + '" /u', '',
                  SW_HIDE, ewWaitUntilTerminated, ResultCode) then
      begin
        Result := 'No se pudo descomprimir SQL Server Express.';
        Exit;
      end;

      if not FileExists(CarpetaSql + '\setup.exe') then
      begin
        Result := 'No se pudo descomprimir SQL Server Express (no aparece setup.exe).' + #13#10 +
                  'Revise que SQLEXPR_x64_ENU.exe sea el paquete offline "Express Core".';
        Exit;
      end;

      { Idioma del paquete: si trae archivos en ingles (1033_ENU_LP) se fuerza /ENU,
        que instala en cualquier Windows. Si no, se instala en el idioma del paquete. }
      if DirExists(CarpetaSql + '\1033_ENU_LP') then
      begin
        ParamIdioma := '/ENU ';
        Log('Paquete de SQL en ingles (1033_ENU_LP), se usa /ENU');
      end
      else
      begin
        ParamIdioma := '';
        Log('ATENCION: el paquete de SQL no trae archivos en ingles, se instala sin /ENU');
      end;

      { Instalacion silenciosa: solo el motor, como instancia por defecto (".") y el usuario actual como administrador.
        SkipRules evita que falle si .NET dejo un reinicio pendiente.
        /INDICATEPROGRESS: escribe el progreso en consola, que se guarda en sqlsetup_consola.txt }
      ParamsSql := '/Q ' + ParamIdioma + '/IACCEPTSQLSERVERLICENSETERMS /ACTION=Install /FEATURES=SQLEngine ' +
                   '/INSTANCENAME=MSSQLSERVER /SQLSYSADMINACCOUNTS="' + UsuarioActual() + '" ' +
                   '/TCPENABLED=1 /BROWSERSVCSTARTUPTYPE=Automatic /UPDATEENABLED=0 ' +
                   '/SKIPRULES=RebootRequiredCheck /INDICATEPROGRESS';
      Log('setup.exe ' + ParamsSql);

      if not Exec(ExpandConstant('{cmd}'),
                  '/C ""' + CarpetaSql + '\setup.exe" ' + ParamsSql +
                  ' > "' + ExpandConstant('{tmp}\sqlsetup_consola.txt') + '" 2>&1"',
                  CarpetaSql, SW_HIDE, ewWaitUntilTerminated, ResultCode) then
      begin
        Result := 'No se pudo ejecutar el instalador de SQL Server Express.';
        Exit;
      end;
      Log('SQL Server Express termino con codigo ' + IntToStr(ResultCode));
      if ResultCode = 3010 then
        NecesitaReinicio := True
      else if ResultCode <> 0 then
      begin
        CarpetaLogs := GuardarLogsSQL();
        Result := 'Error ' + IntToStr(ResultCode) + ' al instalar SQL Server Express.' + #13#10#13#10 +
                  'Se copiaron los logs a:' + #13#10 + CarpetaLogs;
        Exit;
      end;
    end;
  finally
    Progreso.Hide;
  end;
end;

function NeedRestart(): Boolean;
begin
  { Nunca ofrecer reinicio. Si algun componente lo pidio, solo queda en el log. }
  if NecesitaReinicio then
    Log('ATENCION: un componente pidio reinicio, se omite por tratarse de PCs congeladas');
  Result := False;
end;

{ ---------- Configuracion y base de datos (despues de copiar archivos) ---------- }

procedure CurStepChanged(CurStep: TSetupStep);
var
  Instancia, RutaSql, RutaLogSql: String;
  ResultCode: Integer;
  Existe, Crear: Boolean;
begin
  if CurStep <> ssPostInstall then
    Exit;

  Instancia := ObtenerInstancia();
  RutaSql := ExpandConstant('{app}\instalacion\') + ExtractFileName('{#SqlScript}');
  RutaLogSql := ExpandConstant('{app}\instalacion\instalacion_sql.log');
  Log('Instancia elegida: ' + Instancia);

  { 1. Guardar la instancia para que la lea el programa }
  if not SaveStringToFile(ExpandConstant('{app}\conexion.txt'), Instancia, False) then
    SuppressibleMsgBox('No se pudo escribir conexion.txt en la carpeta del programa.', mbError, MB_OK, IDOK);

  { 2. Asegurar que el servicio este corriendo }
  IniciarServicio(Instancia);

  { 3. Elegir con que usuario se trabaja en SQL }
  if not ElegirUsuarioSql(Instancia) then
  begin
    SuppressibleMsgBox('Ningún usuario de esta PC tiene permisos en SQL Server (' + Instancia + ') para crear la base de datos.' + #13#10 +
      'El programa quedó instalado pero sin base. Revise el log de instalación.', mbError, MB_OK, IDOK);
  end
  else
  begin
    { 4. Ver si la base ya existe (codigo 0 = existe) }
    Existe := EjecutarSqlcmd(ArgsSql(Instancia) + ' -b -l 30' +
      ' -Q "IF DB_ID(''{#MyDbName}'') IS NULL RAISERROR(''NoExiste'', 16, 1)"', ResultCode)
      and (ResultCode = 0);

    Crear := True;
    if Existe then
    begin
      if CompareText(ExpandConstant('{param:PISARBD|no}'), 'si') = 0 then
        Crear := True
      else
        { En modo desatendido con /SUPPRESSMSGBOXES responde "No" y conserva la base }
        Crear := SuppressibleMsgBox('La base de datos "{#MyDbName}" ya existe en ' + Instancia + '.' + #13#10#13#10 +
          '¿Desea borrarla y crearla de nuevo? Se perderán los datos actuales.' + #13#10 +
          'Elija "No" para conservar la base existente.', mbConfirmation, MB_YESNO, IDNO) = IDYES;
    end;

    if Crear then
    begin
      { 5. Borrar la base vieja si corresponde }
      if Existe then
        EjecutarSqlcmd(ArgsSql(Instancia) +
          ' -Q "ALTER DATABASE [{#MyDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{#MyDbName}];"', ResultCode);

      { 6. Crear la base con el script (la salida queda en instalacion_sql.log) }
      if (not EjecutarSqlcmd(ArgsSql(Instancia) + ' -b -i "' + RutaSql + '" -o "' + RutaLogSql + '"', ResultCode)) or (ResultCode <> 0) then
        SuppressibleMsgBox('Hubo un error al crear la base de datos (código ' + IntToStr(ResultCode) + ').' + #13#10 +
          'Revise el detalle en: ' + RutaLogSql, mbError, MB_OK, IDOK)
      else
        Log('Base de datos creada correctamente');
    end
    else
      Log('Se conserva la base existente');

    { 7. Dar acceso a la base al usuario que instala y a los usuarios de la PC
         (se prueban los nombres del grupo en ingles y en castellano; el que no exista se ignora) }
    DarAcceso(Instancia, UsuarioActual());
    DarAcceso(Instancia, 'BUILTIN\Users');
    DarAcceso(Instancia, 'BUILTIN\Usuarios');
  end;

  { 8. Limpiar: se borran sqlcmd y el script, queda solo el log }
  DeleteFile(RutaSqlcmd());
  DeleteFile(RutaSql);
end;

procedure DeinitializeSetup();
begin
  if Instancias <> nil then
    Instancias.Free;
end;
