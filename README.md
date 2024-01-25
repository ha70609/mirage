# Mirage

.net core (C#)をベースとしたプロジェクトテンプレート


## Introduction


プロジェクト新規作成時に検討課題となる項目が存在します。  このプロジェクトでは基本となる機能を提供し開発者が可能な限りビジネスロジックに集中できるようにすることを目的とします。

主な機能は次の通りです。

**開発環境**

- VS Code + devcontainerを使用した平準化開発環境
- dindを使用したDB開発環境

**.net**

- WEBプロジェクトサンプル
- テストプロジェクトサンプル
- asp.net identityを使用した認証機能実装例（QRコードをサポートし他2ファクタ認証)


**Infrastructure**

- Github Actionを使用したCI/CDサンプル
- Secret管理サンプル


**今後追加予定**

- sphinxを使用したドキュメント生成サンプル
- DB Migration世代管理サンプル
- DBへのCRUD処理
- クリーンアーキテクチャ実装例
- Github Pageへのドキュメント公開


 ## Documentation

### Tips

#### VS Codeでtasks.json/launch.jsonを自動生成

1. VS Codeで[Ctrl+Shift+P]を入力
2.  入力欄で[Required assets to build and debug]を検索し選択


#### 初回時に必要なコマンド

```
dotnet tool restore
dotnet dev-certs https
```
TODO:Dockerfileで対応できるか確認


#### SQL ServerでIdentity構築
```
rm -rf Web app.sln
dotnet new sln
dotnet new webapp -o Web
dotnet sln add Web
cd Web
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet build
dotnet aspnet-codegenerator identity -dc MirageIdentityDbContext
dotnet ef migrations add -p src/Web CreateIdentitySchema
jq '.ConnectionStrings.MirageIdentityDbContextConnection="Persist Security Info=False;User ID=sa;Password=developer_1;Server=localhost;Initial Catalog=mirage;Encrypt=True;TrustServerCertificate=true;"' src/Web/appsettings.json > src/Web/appsettings.json.cp
rm src/Web/appsettings.json
mv src/Web/appsettings.json.cp src/Web/appsettings.json
dotnet ef database update 
```

#### PostgreSQLでIdentity構築

※dotnet aspnet-codegenerator identity --dcProvider postgresqlがエラーになる。
一度SQLServerでgeneratしてからpostgresでmigrationする
```
rm -rf Web app.sln
dotnet new sln
dotnet new webapp -o src/Web
dotnet sln add src/Web
dotnet add src/Web package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add src/Web package Microsoft.EntityFrameworkCore.Design
dotnet add src/Web package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add src/Web package Microsoft.AspNetCore.Identity.UI
dotnet add src/Web package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/Web package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/Web package Microsoft.EntityFrameworkCore.Tools
dotnet build
dotnet aspnet-codegenerator identity -p src/Web -dc MirageIdentityDbContext
sed -i -e "s/UseSqlServer/UseNpgsql/g" src/Web/Program.cs
dotnet ef migrations add -p src/Web CreateIdentitySchema
dotnet remove src/Web package Microsoft.EntityFrameworkCore.SqlServer
dotnet build
jq '.ConnectionStrings.MirageIdentityDbContextConnection="Server=127.0.0.1; Port=5432; User Id=developer; Password=developer_1; Database=mirage;"' src/Web/appsettings.json > src/Web/appsettings.json.cp
rm src/Web/appsettings.json
mv src/Web/appsettings.json.cp src/Web/appsettings.json
dotnet ef database update -p src/Web
```

#### データベースからEntity Classを作成(dotnet scaffold)

dotnet ef dbcontext scaffold  -c MirageDbContext "Persist Security Info=False;User ID=sa;Password=YourPassword123;Server=localhost;Encrypt=True;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer

 
#### コンテナ上にあるSQL Severに対してCREATE DATABASE実行

sudo docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -P developer_1 -U SA -Q "CREATE DATABASE mirage"

#### devcontainerでGitの認証とホストと共有する

- [Sharing Git credentials with your container](https://code.visualstudio.com/remote/advancedcontainers/sharing-git-credentials)
  - http + credential helperがおすすめ(devcontainerで自動で.gitconfigがコピーされそのまま使用できる)
```
git auth login
git clone https://github.com/ha70609/mirage.git 
git config --global credential.helper store
```

### gitignoreを作る

```
dotnet new gitignore
```

### Links

- [ASP.NET Core プロジェクトでの Identity のスキャフォールディング](
https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-8.0&tabs=netcore-cli)

- [dotnet aspnet-codegenerator](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-8.0)

- [Ubuntu 22.04 に .NET SDK または .NET ランタイムをインストールする](https://learn.microsoft.com/ja-jp/dotnet/core/install/linux-ubuntu-2204)

- [Docker sphinxdoc](https://hub.docker.com/r/sphinxdoc/sphinx)

- [一般的な Web アプリケーション アーキテクチャ](https://learn.microsoft.com/ja-jp/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
  - [ardalis/cleanarchitecture](https://github.com/ardalis/cleanarchitecture)

# 作業メモ
dotnet new classlib -o src/Infrastructure
dotnet new classlib -o src/Application
dotnet new classlib -o src/Domain
dotnet sln add src/Infrastructure
dotnet sln add src/Application
dotnet sln add src/Domain

dotnet new xunit -o tests/Tests.Web
dotnet new xunit -o tests/Tests.Infrastructure
dotnet new xunit -o tests/Tests.Application
dotnet new xunit -o tests/Tests.Domain
dotnet sln add tests/Tests.Web
dotnet sln add tests/Tests.Infrastructure
dotnet sln add tests/Tests.Application
dotnet sln add tests/Tests.Domain

** クリーンアーキテクチャテンプレート **

[こっち(ardalis)](https://github.com/ardalis/cleanarchitecture)より[こっち(jasontaylordev)](https://github.com/jasontaylordev/CleanArchitecture)がよさそう




