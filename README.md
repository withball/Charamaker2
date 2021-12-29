# Charamaker2

これはC#で2Dゲームを作成する際に便利なクラスとか命令とかのセットです。

[できること]<br>
・bmp画像の表示、回転、透明化<br>
・文字の表示、回転、透明化、フェード。フォントは固定。<br>
・bmp画像の集合体Characterとそれを動かすmotionの作成<br>
・Shapeを用いたあたり判定の作成<br>

# License
 
"Charamaker2" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
# Features
C#でゲームと言えばUnityですがあちらは複雑すぎて理解ができませでした。<br>
そんな私が作ったものなので、私と波長が合う人はわかりやすいと思います。<br>
ゲーム制作がほとんどソースコードで完結するのもGoodだと思います。

# Requirement
Vortice 各種
https://github.com/amerkoleci/Vortice.Windows
Microsoft.CodeAnalysys 各種
SharpGen.Runtime +.COM
System.Runtime.CompilerServices.Unsafe 
# Usage

基本はWindowsFormで行います。<br>
だいたいはサンプルに描いてるのがすべてなのでSampleを見てください<br>
filemanのセッティングアップからhyojimanをmakeして.inputinをMouseDownとかに接続します<br>
あとはお好みでEntityとかSceneとかを作ればいいと思います。<br>
ビルドした中身を自分のプログラムのReleaseにコピーしてCharamaker2.exeを参照に追加し、更に同梱されてるSystem.Runtime.CompilerServices.Unsafe.dllも参照に追加すればおｋです。<br>
また含まれているリソースは全て著作権は私のものです。再配布さえしなければ好きに使って構いません。

# Build

Charamaker2.slnを開きます。おそらく依存関係が解決できないと思うのでVorticeとかいろいろnugetでインストールします。必要なバージョンが変でコンソールからバージョン指定しないといけないものもあるので注意してください。そんでビルドできたら万々歳ですね。<br>
だいたいこんな感じ.pngみたいになったら成功です。この画像に乗ってるパッケージをダウンロードしまくればいいわけです。<br>そこにRReaseやsampleの中身をコピペしてやれば大体できます。
