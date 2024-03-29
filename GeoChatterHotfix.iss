; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "GeoChatter"
#define MyAppVersion "v1.2.1.0 Update"
#define MyAppPublisher "NoBuddy&Rhino"
#define MyAppURL "https://geochatter.tv"
#define MyAppExeName "GeoChatter.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{73E30CDF-4BC6-4475-8F74-8B9410FBE613}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
; PrivilegesRequired=lowest
OutputDir=C:\Users\Denis\Desktop\Setup
OutputBaseFilename=GeoChatter_v1.2.1.0_update
SetupIconFile=D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\Resources\logo_clean.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
;AlwaysRestart=yes    

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
[Dirs]
Name: {app}; Permissions: everyone-full
[Files]
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\release\publish\*"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\Release\publish\Styles\*"; DestDir: "{app}\Styles"; Flags: ignoreversion recursesubdirs
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\Release\publish\Scripts\*"; DestDir: "{app}\Scripts"; Flags: ignoreversion recursesubdirs



[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

