# Kanji
간단한 일본 상용한자API를 이용한 한자 정보 가져오기

## kanji 파일
일본 상용한자 2136자의 한자가 입력된 파일

## korean 파일 
kanji 파일에 작성된 한자와 같은 위치에 한국의 뜻과 음이 적혀있고, 뜻과 음이 2개 이상일 경우에는 사이사이 "/"를 추가해 연결함

## 코드
### API 사이트
kanjiapi : https://kanjiapi.dev/란 사이트를 사용해서 일본 상용한자의 정보를 JSON 파일로 가져오기
```
https://kanjiapi.dev/v1/kanji/{한자} 
https://kanjiapi.dev/v1/reading/{발음}
https://kanjiapi.dev/v1/grade-{학년}
https://kanjiapi.dev/v1/word/{한자}
```

