path "C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"

svcutil "http://localhost:14006/GsmMessageService.svc?wsdl"^
        /o:"Wodeyun.Project.GsmMessage.Wrappers\GsmMessageWrapper.cs"^
        /r:"..\..\Wodeyun.Gf\Wodeyun.Gf.Entities\bin\Debug\Wodeyun.Gf.Entities.dll"^
        /n:*,Wodeyun.Project.GsmMessage.Wrappers^
        /syncOnly^
        /noConfig
				
svcutil "http://localhost:14006/GsmItemService.svc?wsdl"^
        /o:"Wodeyun.Project.GsmMessage.Wrappers\GsmItemWrapper.cs"^
        /r:"..\..\Wodeyun.Gf\Wodeyun.Gf.Entities\bin\Debug\Wodeyun.Gf.Entities.dll"^
        /n:*,Wodeyun.Project.GsmMessage.Wrappers^
        /syncOnly^
        /noConfig
				
path "..\..\Wodeyun.Gf\Wodeyun.Gf.Tools\Wodeyun.Gf.Tools.SvcFixer\bin\Debug"

Wodeyun.Gf.Tools.SvcFixer "Wodeyun.Project.GsmMessage.Wrappers\GsmMessageWrapper.cs"
Wodeyun.Gf.Tools.SvcFixer "Wodeyun.Project.GsmMessage.Wrappers\GsmItemWrapper.cs"
