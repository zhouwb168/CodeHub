path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodMachine.Services"
appcmd delete site "Wodeyun.Project.WoodMachine.Services"

appcmd delete apppool "Wodeyun.Project.WoodMachine.Web"
appcmd delete site "Wodeyun.Project.WoodMachine.Web"
