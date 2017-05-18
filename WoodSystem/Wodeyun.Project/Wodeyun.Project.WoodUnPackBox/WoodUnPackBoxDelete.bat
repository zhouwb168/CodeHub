path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodUnPackBox.Services"
appcmd delete site "Wodeyun.Project.WoodUnPackBox.Services"

appcmd delete apppool "Wodeyun.Project.WoodUnPackBox.Web"
appcmd delete site "Wodeyun.Project.WoodUnPackBox.Web"
