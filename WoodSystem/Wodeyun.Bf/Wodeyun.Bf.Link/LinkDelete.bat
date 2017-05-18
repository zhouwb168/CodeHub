path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Link.Services"
appcmd delete site "Wodeyun.Bf.Link.Services"

appcmd delete apppool "Wodeyun.Bf.Link.Web"
appcmd delete site "Wodeyun.Bf.Link.Web"
