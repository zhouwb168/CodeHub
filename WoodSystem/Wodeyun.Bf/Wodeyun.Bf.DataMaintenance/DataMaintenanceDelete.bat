path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.DataMaintenance.Services"
appcmd delete site "Wodeyun.Bf.DataMaintenance.Services"

appcmd delete apppool "Wodeyun.Bf.DataMaintenance.Web"
appcmd delete site "Wodeyun.Bf.DataMaintenance.Web"
