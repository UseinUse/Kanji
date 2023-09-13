# 한자
간단한 일본 상용한자API를 이용한 한자 정보 가져오기 <br/>

## kanji 파일
일본 상용한자 2136자의 한자가 입력된 파일 <br/>

## korean 파일 
kanji 파일에 작성된 한자와 같은 위치에 한국의 뜻과 음이 적혀있고, 뜻과 음이 2개 이상일 경우에는 사이사이 "/"를 추가해 연결함 <br/>

## 코드
### API 사이트
kanjiapi : https://kanjiapi.dev/란 사이트를 사용해서 일본 상용한자의 정보를 JSON 파일로 가져온다.
```
https://kanjiapi.dev/v1/kanji/{한자} 
https://kanjiapi.dev/v1/reading/{발음}
https://kanjiapi.dev/v1/grade-{학년}
https://kanjiapi.dev/v1/word/{한자}
```
위의 주소로 들어가면 JSON 파일을 받을 수 있다. <br/>

### 한자 정보
이 프로그램에서는 한자, 획수, JLPT 등급, 의미, 훈독, 음독, 이름을 읽는 방법을 가져온다.
그래서 추가로 한자 뜻, 음 파일을 더했다. <br/>

### 사용법
키보드의 아무 키를 입력하면 한자 정보를 가지고 있는 딕셔너리에서 한자 정보가 입력된 값을
찾아 콘솔창에 출력해준다. <br/>

그리고 ESC키를 누르면 프로그램은 종료된다. <br/>

