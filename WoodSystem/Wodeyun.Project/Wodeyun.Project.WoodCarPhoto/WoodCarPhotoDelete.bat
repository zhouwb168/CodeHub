path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCarPhoto.Services"
appcmd delete site "Wodeyun.Project.WoodCarPhoto.Services"

appcmd delete apppool "Wodeyun.Project.WoodCarPhoto.Web"
appcmd delete site "Wodeyun.Project.WoodCarPhoto.Web"
