cd Wodeyun.Project.GsmArea
call GsmAreaWeb.bat
cd..

cd Wodeyun.Project.GsmLine
call GsmLineWeb.bat
cd..

cd Wodeyun.Project.GsmMessage
call GsmMessageWeb.bat
cd..

cd Wodeyun.Project.GsmOrigin
call GsmOriginWeb.bat
cd..

cd Wodeyun.Project.GsmReport
call GsmReportWeb.bat
cd..

cd Wodeyun.Project.GsmSupplier
call GsmSupplierWeb.bat
cd..

cd Wodeyun.Project.GsmTree
call GsmTreeWeb.bat
cd..

cd Wodeyun.Project.Barrier
call BarrierWeb.bat
cd..

cd Wodeyun.Project.Check
call CheckWeb.bat
cd..

cd Wodeyun.Project.EmptyPound
call EmptyPoundWeb.bat
cd..

cd Wodeyun.Project.Factory
call FactoryWeb.bat
cd..

cd Wodeyun.Project.FullPound
call FullPoundWeb.bat
cd..

cd Wodeyun.Project.Recover
call RecoverWeb.bat
cd..

cd Wodeyun.Project.Wood
call WoodWeb.bat
cd..

cd Wodeyun.Project.WoodCard
call WoodCardWeb.bat
cd..

cd Wodeyun.Project.WoodCarPhoto
call WoodCarPhotoWeb.bat
cd..

cd Wodeyun.Project.WoodDataList
call WoodDataListWeb.bat
cd..

cd Wodeyun.Project.WoodFinance
call WoodFinanceWeb.bat
cd..

cd Wodeyun.Project.WoodGps
call WoodGpsWeb.bat
cd..

cd Wodeyun.Project.WoodJoin
call WoodJoinWeb.bat
cd..

cd Wodeyun.Project.WoodLaboratory
call WoodLaboratoryWeb.bat
cd..

cd Wodeyun.Project.WoodLaboratoryConfirme
call WoodLaboratoryConfirmeWeb.bat
cd..

cd Wodeyun.Project.WoodMachine
call WoodMachineWeb.bat
cd..

cd Wodeyun.Project.WoodPackBox
call WoodPackBoxWeb.bat
cd..

cd Wodeyun.Project.WoodPowerOfGps
call WoodPowerOfGpsWeb.bat
cd..

cd Wodeyun.Project.WoodPowerOfReadCard
call WoodPowerOfReadCardWeb.bat
cd..

cd Wodeyun.Project.WoodReport
call WoodReportWeb.bat
cd..

cd Wodeyun.Project.WoodResetCard
call WoodResetCardWeb.bat
cd..

cd Wodeyun.Project.WoodUnPackBox
call WoodUnPackBoxWeb.bat
cd..

cd Wodeyun.Project.WoodPrice
call WoodPriceWeb.bat
cd..

cd Wodeyun.Project.GsmJoin
call GsmJoinWeb.bat
cd..


path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Www"
appcmd delete site "Wodeyun.Project.Www"

appcmd add apppool /name:"Wodeyun.Project.Www"
appcmd set apppool "Wodeyun.Project.Www"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Www"^
                /bindings:http/*:11004:^
                /physicalPath:"%cd%\Wodeyun.Project.Www"
appcmd set site "Wodeyun.Project.Www"^
                /[path='/'].applicationPool:"Wodeyun.Project.Www"
