path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Wood.Services"
appcmd delete site "Wodeyun.Project.Wood.Services"

appcmd delete apppool "Wodeyun.Project.Wood.Web"
appcmd delete site "Wodeyun.Project.Wood.Web"
