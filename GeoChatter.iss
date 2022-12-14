; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "GeoChatter"
#define MyAppVersion "1.2.0.0"
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
;PrivilegesRequired=lowest
OutputDir=C:\Users\Denis\Desktop\Setup
OutputBaseFilename=GeoChatter_v1.2.0.0
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
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\Release\publish\*"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\Release\publish\Styles\*"; DestDir: "{app}\Styles"; Flags: ignoreversion recursesubdirs
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\GeoChatter\GeoChatter\bin\Release\publish\Scripts\*"; DestDir: "{app}\Scripts"; Flags: ignoreversion recursesubdirs
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\Dependencies\Quickstart.pdf"; DestDir: "{app}"; Flags: ignoreversion 
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\Dependencies\windowsdesktop-runtime-6.0.3-win-x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall dontcopy
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\Dependencies\aspnetcore-runtime-6.0.3-win-x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall dontcopy 
Source: "D:\Projects\Github\GeoChatter\GeoChatter-Client\Dependencies\VC_redist.x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall dontcopy 

; NOTE: Don't use "Flags: ignoreversion" on any shared system files



[Code]
var
  NeedVC: Boolean;
  NeedNet : Boolean;
  NeedAsp: Boolean;
  VCVersion:String;
  aspNames : array of String;
  Names: array of String;
  Name : String;
  I : integer;
  changed: integer;
  changed2: integer;

procedure Explode(var Dest: TArrayOfString; Text: String; Separator: String);
var
  i, p: Integer;
begin
  i := 0;
  repeat
    SetArrayLength(Dest, i+1);
    p := Pos(Separator,Text);
    if p > 0 then begin
      Dest[i] := Copy(Text, 1, p-1);
      Text := Copy(Text, p + Length(Separator), Length(Text));
      i := i + 1;
    end else begin
      Dest[i] := Text;
      Text := '';
    end;
  until Length(Text)=0;
end;

function string_to_numerical_value(AString: string; AMaxVersion: LongWord): LongWord;
var
  InsidePart: boolean;
  NewPart: LongWord;
  CharIndex: integer;
  c: char;
begin
  Result := 0;
  InsidePart := FALSE;
  // this assumes decimal version numbers !!!
  for CharIndex := 1 to Length(AString) do begin
    c := AString[CharIndex];
    if (c >= '0') and (c <= '9') then begin
      // new digit found
      if not InsidePart then begin
        Result := Result * AMaxVersion + NewPart;
        NewPart := 0;
        InsidePart := TRUE;
      end;
      NewPart := NewPart * 10 + Ord(c) - Ord('0');
    end else
      InsidePart := FALSE;
  end;
  // if last char was a digit the last part hasn't been added yet
  if InsidePart then
    Result := Result * AMaxVersion + NewPart;
end;




function VC2017RedistNeedsInstall: Boolean;
begin
  
  if (NeedVC) then
  begin
    ExtractTemporaryFile('VC_redist.x64.exe');
  end;
  Result := NeedVC;
end;

function DesktopRedistNeedsInstall: Boolean;
begin
  
  if (NeedNet) then
  begin
    ExtractTemporaryFile('windowsdesktop-runtime-6.0.3-win-x64.exe');
  end;
  Result := NeedNet;
end;

function AspRedistNeedsInstall: Boolean;
begin
  Log('AspRedistNeedsInstall:');
  if (NeedAsp) then
  begin
    Log('Installing asp.net');
    ExtractTemporaryFile('aspnetcore-runtime-6.0.3-win-x64.exe');
  end
  else
  begin
    Log('asp.net not needed');
  end;
  Result := NeedAsp;
  Log('AspRedistNeedsInstall DONE');
end;

function VCinstalled: Boolean;
 // By Michael Weiner <mailto:spam@cogit.net>
 // Function for Inno Setup Compiler
 // 13 November 2015
 // Returns True if Microsoft Visual C++ Redistributable is installed, otherwise False.
 // The programmer may set the year of redistributable to find; see below.
 var
  names: TArrayOfString;
  i: Integer;
  dName, key, year: String;
 begin
  Log('VCInstalled');
  // Year of redistributable to find; leave null to find installation for any year.
  year := '2015-2022';
  Result := True;
  key := 'Software\Microsoft\Windows\CurrentVersion\Uninstall';
  // Get an array of all of the uninstall subkey names.
  if RegGetSubkeyNames(HKEY_LOCAL_MACHINE, key, names) then
   // Uninstall subkey names were found.
   begin
    i := 0
    while ((i < GetArrayLength(names)) and (Result = True)) do
     // The loop will end as soon as one instance of a Visual C++ redistributable is found.
     begin
      // For each uninstall subkey, look for a DisplayName value.
      // If not found, then the subkey name will be used instead.
      if not RegQueryStringValue(HKEY_LOCAL_MACHINE, key + '\' + names[i], 'DisplayName', dName) then
       dName := names[i];
      // See if the value contains both of the strings below.
      Result := (Pos(Trim('Visual C++ ' + year),dName) * Pos('Redistributable',dName) = 0)
      i := i + 1;
     end;
   end;
   Log('VCInstalled:' + dName);
 end;

function NeedRestart(): Boolean;
begin
  Log('NeedRestart');
  //if RegQueryStringValue(HKEY_LOCAL_MACHINE,
  //     'SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeAdditional', 'Version', VCVersion) then
  //begin
  //  Log('VC Redist Version check : found ' + VCVersion);
  //  NeedVC := (CompareStr(VCVersion, '14.31.31103')<0);
  //end
  //else 
  //begin
  //   Log('VC Redist Version check : not found');
  //  NeedVC := True;
  //end;
  NeedVC := VCInstalled;
  Log('NeedVC :' + IntToStr(Integer(NeedVC)));
  if (RegGetValueNames(HKEY_LOCAL_MACHINE, 'SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.NETCore.App', Names)) then
  begin
     Log('Core Version check : FOUND ENTRIES: ' + IntToStr(GetArrayLength(Names)));
    for I := 0 to GetArrayLength(Names) - 1 do
    begin
      Name := Names[I];
      if (string_to_numerical_value(Name,10) >= string_to_numerical_value('6.0.3',10)) then
      begin
        Log('Core Version check : found ' + Name);
        NeedNet := False;
        Break;
      end
      else
      begin
         Log('Core Version check : not found ' + Name);
         NeedNet := True;
      end;
    end;
  end
  else
  begin
    Log('Core Version check : Could not read reg');
    NeedNet := True;
  end;
   Log('NeedNet :' + IntToStr(Integer(NeedNet)));
  
  
  
  
  
  if (RegGetSubkeyNames(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Updates\.NET', aspNames)) then
  begin
     Log('asp Version check : FOUND ENTRIES: ' + IntToStr(GetArrayLength(aspNames)));
    for I := 0 to GetArrayLength(aspNames) - 1 do
    begin
      Name := aspNames[I];
      Log('NAME: ' + Name);
      changed:= StringChangeEx(Name, 'Microsoft ASP.NET Core ', '', True);
      changed2 := StringChangeEx(Name, ' - Shared Framework (x64)', '', True);
      if(changed + changed2= 2 ) then
      begin
        Log('Version: ' + Name);
        if (string_to_numerical_value(Name,10) >= string_to_numerical_value('6.0.3',10)) then
        begin
          Log('asp Version check : found ' + Name);
          NeedAsp := False;
          Break;
        end
        else
        begin
           Log('asp Version check : not found ' + Name);
           NeedAsp := True;
        end;
      end
      else
      begin
        Log('Wrong folder: ' + Name);
      end;
    end;
  end
  else
  begin
    Log('asp Version check : Could not read reg');
    NeedAsp := True;
  end;
      Log('NeedAsp :' + IntToStr(Integer(NeedAsp)));
  
  Result := (NeedAsp or NeedNet or NeedVC);
  Log('NeedRestart result: '+IntToStr(Integer(Result)));
end;

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\VC_redist.x64.exe"; Parameters: "/install /passive /norestart"; Check: VC2017RedistNeedsInstall ; Flags: waituntilterminated
Filename: "{tmp}\aspnetcore-runtime-6.0.3-win-x64.exe"; Parameters: "/install /passive /norestart"; Check: AspRedistNeedsInstall ; Flags: waituntilterminated
Filename: "{tmp}\windowsdesktop-runtime-6.0.3-win-x64.exe"; Parameters: "/install /passive /norestart"; Check: DesktopRedistNeedsInstall ; Flags: waituntilterminated
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
Filename: "{app}\QuickStart.pdf"; Description: "Open quickstart guide"; Flags: nowait postinstall skipifsilent shellexec runasoriginaluser

