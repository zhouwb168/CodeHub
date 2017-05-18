path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmArea.Services"
appcmd delete site "Wodeyun.Project.GsmArea.Services"

appcmd delete apppool "Wodeyun.Project.GsmArea.Web"
appcmd delete site "Wodeyun.Project.GsmArea.Web"
