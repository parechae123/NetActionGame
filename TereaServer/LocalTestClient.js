const io = require('socket.io-client');

// Socket.IO 서버 URL
const socket = io('http://localhost:3000'); // 여기에는 해당하는 서버의 URL을 작성해야 합니다.

// 연결됐을 때
socket.on('connect', () => {
  console.log('Connected to server');
  // 추가 작업 가능
});

// 서버로부터 메시지 받으면 실행
socket.on('chat message', (msg) => {
  console.log('Received message:', msg);
  // 메시지 수신 후 추가 작업 가능
});

// 예시: 클라이언트에서 서버로 메시지 보내기
function sendMessage(message) {
  socket.emit('chat message', message);
}

// 예시 메시지 보내기
sendMessage('Hello from client');