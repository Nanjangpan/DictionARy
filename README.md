# Korean Alphabet Education Program, DictionARy
Augmented Reality (AR, 증강현실) 을 이용한 한글 교육 프로그램

### 연구주제
  이 연구는 AR (증강현실, Augmented Reality) 을 이용하여 한글의 자모음 결합 원리의 이해를 돕는 안드로이드 모바일 어플리케이션 개발을 최종 목표로 한다. Unity 의 Vuforia 엔진을 이용하여 한글 이미지 카드를 인식하고, 한글을 음절로, 음절을 단어로 변환하여 (예: ㄱ, ㅏ,ㅂ,ㅏ,ㅇ → 가방) 단어장에 저장하거나 이미지 검색을 할 수 있도록 한다. Unity 게임개발 툴, Vuforia 엔진에 대한 이해, 데이터 베이스와 구글 API 연동에 대한 기술을 필요로 한다.
##

### 연구의 필요성
  AR 기술은 오늘날 자율주행 자동차, 모바일 게임, 인테리어 시뮬레이션 등 많은 분야에서 활용되고 있다. 교육 분야도 예외는 아니며, 특히 흥미요소가 많이 필요한 유아 언어 교육이나 3차원 시뮬레이션이 필요한 자연 과학 분야에서 많이 사용되고 있다. 그러나 한글 교육에 있어서 기존의 AR 교육 프로그램은 간단한 단어 설명이나 QR 코드를 이용하여 동화책의 그림을 3D 로 보여주는 정도에 그친다. 따라서 이 연구에서는 한글 자모음의 구성과 결합 원리를 AR 로 구현하고 데이터 베이스 연동을 통해 단어장 기능을, 구글 API 를 이용하여 검색 기능을 구현하여  AR 활용 사례를 확장하고자 한다. 
##
### 연구 내용
#### 연구 환경
- 개발 환경 : Unity 3.12, Vuforia Engine 8.1.10, Android 8.0
- 실험 환경 : Samsung Galaxy Tab A 8.0 
- 실험 준비물 : Android 기반 기기, 한글 카드 
##
#### 한글 자모음 음절화 알고리즘
인식한 한글 카드 중점의 좌표를 이용하여 한글의 자모음 카드가 결합되었을 때 그것을 글자로 인식하는 과정을 설계한다. 설계 내용은 아래와 같다. <br>
1. 인식된 모든 모음의 좌표를 받고, 모음의 거리가 일정 기준 이하가 되고 모음결합 조건에 부합한다면 하나의 모음으로 인식한다.  <br>
ex) ㅏ + ㅣ = ㅐ, ㅗ + ㅏ = ㅘ 
2. 각각의 모음마다 중점을 기준으로 원 형태의 경계를 그린다.
3. 만약 그 원의 경계 안에 자음이 들어온다면 모음과 결합한다.
4. 결합 규칙 두가지를 정의한다. 
  a)	group1 - ㅏ,ㅑ,ㅓ,ㅕ,ㅣ,ㅐ,ㅒ,ㅔ,ㅖ
    ex) 가, 각, 거, 걱...
  b)	group2 - ㅗ,ㅛ,ㅜ,ㅠ,ㅡ,ㅘ,ㅚ,ㅝ,ㅟ,ㅢ,ㅙ,ㅞ 
    ex)고, 곡, 구, 국 

