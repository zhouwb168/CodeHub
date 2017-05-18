path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPackBox.Services"
appcmd delete site "Wodeyun.Project.WoodPackBox.Services"

appcmd delete apppool "Wodeyun.Project.WoodPackBox.Web"
appcmd delete site "Wodeyun.Project.WoodPackBox.Web"
