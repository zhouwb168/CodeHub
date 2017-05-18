path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Factory.Services"
appcmd delete site "Wodeyun.Project.Factory.Services"

appcmd delete apppool "Wodeyun.Project.Factory.Web"
appcmd delete site "Wodeyun.Project.Factory.Web"
