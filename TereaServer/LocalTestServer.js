const express = require('express');
const http = require('http');
const socketIO = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIO(server);
let UserList = [];


// 정적 파일 제공을 위해 public 폴더를 사용합니다.
app.use(express.static(__dirname + '/public'));

// 클라이언트가 Socket.IO에 연결될 때 실행됩니다.
io.on('connection', (socket) => {
  console.log('a user connected');

  // 클라이언트로부터 'chat message' 이벤트를 수신합니다.
  socket.on('chat message', (msg) => {
    console.log('message: ' + msg);
    
    // 모든 연결된 클라이언트에게 'chat message' 이벤트를 방출합니다.
    io.emit('chat message',msg);
    //socketIO에서는 앞에 있는 emit(주소,자료)형태로 전송해줌
  });

  // 클라이언트가 연결을 종료할 때 실행됩니다.
  socket.on('disconnect', () => {
    console.log('user disconnected');
  });
  socket.on('connectUser',()=>{
    const tempName = genKey(8);
    console.log("유저등록 : ");
    UserList.push(tempName);
    io.emit('connectUser',tempName);
  });
});

// 서버를 3000번 포트에서 실행합니다.
server.listen(3000, () => {
  console.log('Server listening on port 3000');
});

function genKey(length){
  let result = '';
  const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

  for(let i = 0; i< length; i++)
  {
      result += characters.charAt(Math.floor(Math.random()* characters.length));
  }
  return result;
}