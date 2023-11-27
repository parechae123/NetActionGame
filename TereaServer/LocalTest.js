const WebSocket = require('ws');
const wss = WebSocket.Server({port:8000},()=>{
    console.log('서버 시작');
});

const userList =[];


wss.on('connection', function connections(ws){      //커넥션이 되었을 때
    ws.clientID = genKey(8);

    ws.on('message',(data) =>{
        const jsonData = JSON.parse(data);
        console.log('받은 데이터 : ', jsonData);

        if(jsonData.requestType == 10)
        {
            ws.send(JSON.stringify(userList));
        }
        else if(jsonData.requestType == 20)
        {
            ws.send(JSON.stringify(userList));
        }

        wss.clients.forEach((client)=>
        {
            client.send(data);
        })
    });

    ws.on('close',()=>{
        const index = userList.indexOf(ws.clientID);
        if(index !== -1)
        {
            console.log('클라이언트가 해제됨 - ID : ',ws.clientID);
            userList.splice(index,1);
        } 
    });
    //새로 연결 된 클라이언트를 유저 리스트에 추가
    userList.push(ws.clientID);
    ws.send(JSON.stringify({clientID : ws.clientID}));
    console.log('클라이언트 연결 - ID : ', ws.clientID);
})
wss.on('listening',()=>{
    console.log('리스닝...');
})

function genKey(length){
    let result = '';
    const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    for(let i = 0; i< length; i++)
    {
        result += characters.charAt(Math.floor(Math.random()* characters.length));
    }
    return result;
}