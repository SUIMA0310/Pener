# Pener
Penguins lab Mod installer.

# 再構築案
## 基本設計
* Pener.Core
* Pener.Client.Core
* Pener.Client.Cli
* Pener.Client.Web
* Pener.Client.Desktop
* Pener.Server.Core
* Pener.Server.Cli
* Pener.Server

### Pener.Core
ServerとClient間の通信で利用されるモデルを定義する  
また、コンバーター等のサーバ・クライアント双方から利用するユーティリティもここへ実装する

### Pener.Client.Core
Client側のバックエンドサービスを実装する  
主にServerとの通信に関するサービスをここで実装する

* JwtService (Tokenの保存と検証)
* AuthService (ServerからのTokenの取得)
* UserService (ユーザの管理)
* PackageService (Packageの管理)
* FileService (リモートFileの管理)

### Pener.Client.Cli
Penerに関する完全な操作を可能にする  
主に管理者向け

### Pener.Client.Web
ユーザ及びパッケージに関する操作を可能にする  

### Pener.Client.Desktop
主にパッケージの展開を目的とする  
使用者は一般ユーザを想定

### Pener.Server.Core
Server側のバックエンドサービスを実装する
主にDBやファイルシステムとの連携をここで実装する

* JwtService (Tokenの発行/検証)
* AuthService (ユーザ名/パスワードの検証)
* UserService (ユーザの管理)
* PackageService (Packageの管理)
* FileService (Fileの管理)

### Pener.Server.Cli
Serverの管理用に認証無しの特権動作を実現する  
利用は、Serverのローカル上に限定する

### Pener.Server
WebApiサーバ  
Pener.Server.CoreのサービスにApiを付与することが主目的