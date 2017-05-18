path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmSupplier.Services"
appcmd delete site "Wodeyun.Project.GsmSupplier.Services"

appcmd delete apppool "Wodeyun.Project.GsmSupplier.Web"
appcmd delete site "Wodeyun.Project.GsmSupplier.Web"
