path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Barrier.Services"
appcmd delete site "Wodeyun.Project.Barrier.Services"

appcmd delete apppool "Wodeyun.Project.Barrier.Web"
appcmd delete site "Wodeyun.Project.Barrier.Web"
