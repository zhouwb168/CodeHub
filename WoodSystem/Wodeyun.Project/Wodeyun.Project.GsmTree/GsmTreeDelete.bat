path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmTree.Services"
appcmd delete site "Wodeyun.Project.GsmTree.Services"

appcmd delete apppool "Wodeyun.Project.GsmTree.Web"
appcmd delete site "Wodeyun.Project.GsmTree.Web"
