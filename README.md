# NoR2252

## Clone時的注意事項

匯入FirebaseSDK (FirebaseStorage)選擇dotnet4的版本  
```
Unity 2020中缺少Firebase Android配置文件。
為了支持無法自定義Gradle構建的Unity版本，Firebase編輯器工具將Assets/Plugins/Android/Firebase/res/values/google-services.xml成為Android資源，並將其打包為Android構建，以便Firebase SDK可以使用它來初始化默認的FirebaseApp實例。

在Unity 2020中，所有Android資源必須位於帶有.androidlib後綴的目錄中。如果您的項目使用的是可生成Assets/Plugins/Android/Firebase目錄的Firebase SDK，請將其重命名為Assets/Plugins/Android/Firebase.androidlib 。確保它包含AndroidManifest.xml ， project.properties和res/values/google-services.xml 。
```

下載google-service.xml 並放到路徑 `Assets\Plugins\Android\Firebase.androidlib\res\values`

[參考資料](https://firebase.google.com/docs/unity/setup?authuser=0#add-sdks)