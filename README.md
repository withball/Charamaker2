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


# Build

Charamaker2.slnを開きます。そして開始を押すなりCtrl＋Bなりでビルドをします。その時におそらくエラーが出るでしょう。それを解消すればよいのです。<br>
コンポーネントのインストールは割と自動でやってくれるようです。<br>
X.resxが開けません見たいなエラーが出るのでエクスプローラーからファイルをいじります。そのファイルXのプロパティから全般の属性の下にあるWebなんたらを許可するにチェックを入れて適用します。<br>
そしたらビルドが成功して実行したら青い変な画面が出るはずです。<br>

ビルドファイルの中身がだいたいこんな感じ.pngみたいになったら成功です。<br>


# Usage

基本はWindowsFormで行います。<br>
だいたいはサンプルに描いてるのがすべてなのでSampleを見てください<br>
filemanのセッティングアップからhyojimanをmakeして.inputinをMouseDownとかに接続します<br>
あとはお好みでEntityとかSceneとかを作ればいいと思います。<br>
ビルドした中身を自分のプログラムのReleaseにコピーします。<br>
Charamaker2.exeをソリューションエクスプローラーから参照に追加し、更に同梱されてるSystem.Runtime.CompilerServices.Unsafe.dllも参照に追加すればビルドができるようになると思います<br>
また含まれているリソースは全て著作権は私のものです。再配布さえしなければ煮るなり焼くなりどうぞ。

# エラーとか
～～の参照を解決できませんみたいなエラーは実行ファイルの隣にdllか何かが足りない時に発生するはずです。どこかからそのdllを探して実行ファイル横にぶち込んでおきましょう。<br>
filemanを使用する際にSystem.Numericsが解決できませんみたいなエラーが発生することがあります。そうすると出力のところにapp.configをいじってくださいみたいなメッセージが出るのでそれに従っていじると直ります。


# Tinamini

どんなゲームが作れるかはWith_Ball_Dribbleのプロフィールのリンクからご覧ください。

# AboutTemplate

基本的なEntityとSceneのクラスを構えているGameSet1を作りました。<br>
それに際してTemplateを用意しました。使えるようにするには
<br>
\Visual Studio 2019\Templates\ProjectTemplates
<br>
にGameSet1Project.zipを置いてください。<br>
インストールとかしてないので参照がおかしなことになってると思います。なのでDDebug内の同名の奴を参照してあげてください<br>
Resourceの中にリソースフォルダ群を置いといてあるのでそちらも.exeの横においてご活用ください。


# About通信
P2P通信を可能にするやつを作りました。仕様上サーバーからクライアントにhyojimanをクライアントがサーバーにinputinを送るだけですが割と何とかなると思います。<br>
通信はIPなど含まれたメッセージを何かしらの手段で相手に伝え、相手がそれを入力、つまり人力シェイクハンドで行います。なので通信するためにはSTUNサーバー以外必要ありません。<br>
こちらの機能はMicrosoft.MixedReality.WebRTCを使用しております。<br>




