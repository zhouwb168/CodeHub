path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Role.Services"
appcmd delete site "Wodeyun.Bf.Role.Services"

appcmd delete apppool "Wodeyun.Bf.Role.Web"
appcmd delete site "Wodeyun.Bf.Role.Web"
