path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPrice.Services"
appcmd delete site "Wodeyun.Project.WoodPrice.Services"

appcmd delete apppool "Wodeyun.Project.WoodPrice.Web"
appcmd delete site "Wodeyun.Project.WoodPrice.Web"
