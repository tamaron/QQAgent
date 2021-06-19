# QQAgent
 
QQAgentはUnity(C#)製の簡易バーチャルアシスタントアプリケーションです。

# DEMO

![Videotogif-1](https://user-images.githubusercontent.com/49875900/122632255-a0a65480-d10c-11eb-9535-0198cf44eb86.gif) 

# Features
 
ユーザーからの入力を形態素解析エンジンMecabで解析し、その結果をもとに応答の生成を行います。Google Cloud Speech-To-Textを利用した音声入力、OpenWeatherMapを利用した天気予報の検索、動的計画法を用いただじゃれ探索などの機能を備えています。

# Dependency
Unity 2020.3.5f1(以上)
 
# ClassDiagram 
[Diagram(.svg)](Assets/docs/QQAgentInputOutputDiagram.svg)

# NOTE
一般には公開されていないWebAPIを用いて動作しているため、現在ローカルでビルドすることはできません。
