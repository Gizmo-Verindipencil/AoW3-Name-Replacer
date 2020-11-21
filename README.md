AOW3 Name Replacer
===

![image](Image/title.png)

## 導入
Age of Wonders IIIは、オランダのTriumph Studiosが開発したターン性ストラテジーゲームです。このゲームには指導者と呼ばれる様々なヒーローユニットが登場します。人間、ゴブリン、エルフ...といったバリエーションがありますが、既に用意されたものの他に、自分でカスタマイズしたヒーローを作ることができます。作ったヒーローには、名前をつけることができます。しかし残念ながら
日本語を（正確にはマルチバイト文字を）名前に使うことができません。

## 概要
指導者が保存されているファイルを、バイナリエディタで直接編集することで、日本語の名前の指導者をつくることができます。これは、日本語Wikiの[カスタム指導者の日本語化](https://aow3.swiki.jp/index.php?%E8%B3%BC%E5%85%A5%E5%BE%8CFAQ)でも解説されています。しかしながら「バイナリエディタを使って」「目的の部分を探して」「書き換える」というのは、少し面倒です。

## 手順
1. ゲームを起動して、カスタム指導者を作ります。
![手順1](Image/01.png)

2. ゲームを終了します。

3. Aow3-Name-Replace.exeを実行します。

4. 任意のProfileファイルを対象に、[1]で作成したカスタム指導者の名前、新しい名前（<font color="Pink">※変更前の名前と同じ文字数である必要あり</font>）を指定します。  
<font color="Pink">※Profileファイルは「C:\Users\(your name)\Documents\My Games\AoW3\Profiles」に保存されています。</font>
![手順4](Image/02.png)

5. ゲームを起動して、名前が置き換わったことを確認してください。
![手順5](Image/03.png)

<font color="Pink">!!! 問題が起きた場合、File Pathで指定したProfileファイルと同じ場所に「{ ユーザー }.APD.backup」という名前で、元ファイルがバックアップされています。このツールで変更されたAPDファイルを削除し、バックアップファイルを元ファイルの内容にリネームしてください。</font>

## 留意事項
- 置換対象の名前と新しい名前は同じ文字数にする必要があります。  
    
    例：  

    |既存の名前|新しい名前|結果|
    |:---|:---|:---|
    |abcde|あいうえお|あいうえお|
    |abcde|あいう|あいうXXX|
    |abc|あいうえお|あいうえおXXX|

    <font color="Pink">__※XXXは文字化けした表現を指します。__</font>
    
- 置換の処理は2番目の名前、1番目の名前の順番で行われます。  
  その為、指定した内容によっては次のような意図しない発生が起きます。

    例：  
    1番目の名前：カレーライス ⇒ ハヤシライス  
    2番目の名前：カレー ⇒ うどん  
    
    × このようにはならない：

    |1番目の名前|2番目の名前| |1番目の名前|2番目の名前|
    |:---|:---|:---:|:---|:---|
    |カレーライス|カレー|⇒|ハヤシライス|うどん|

    ○ 実際にはこのようになる：
    
    |1番目の名前|2番目の名前| |1番目の名前|2番目の名前|
    |:---|:---|:---:|:---|:---|
    |カレーライス|カレー|⇒|うどんライス|うどん|

- このツールは単純なバイナリ置換を行います。その為、最初に置き換える名前次第ではファイルが破損することがあります。例えば、指導者の髪/ポーズの設定に関する内容の全体、あるいは一部に該当する名前は、この問題を引き起こします。なので置換対象の名前は「xxxx」とか「abcdef」のような、意味のなさそうな内容にすることを推奨します。

## 参考
- [Steam - Age of Wonders III](https://store.steampowered.com/app/226840/Age_of_Wonders_III/)
- [カスタム指導者の日本語化](https://aow3.swiki.jp/index.php?%E8%B3%BC%E5%85%A5%E5%BE%8CFAQ)