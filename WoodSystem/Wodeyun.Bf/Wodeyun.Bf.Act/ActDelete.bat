path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Act.Services"
appcmd delete site "Wodeyun.Bf.Act.Services"

appcmd delete apppool "Wodeyun.Bf.Act.Web"
appcmd delete site "Wodeyun.Bf.Act.Web"
