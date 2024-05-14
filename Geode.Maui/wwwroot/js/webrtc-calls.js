const configuration = {
    iceServers: [
        {
            "urls": ["stun:stun.l.google.com:19302",
                "stun:stun1.l.google.com:19302",
                "stun:stun2.l.google.com:19302"]
        }
    ]
}

let rtcHub
let localStream
let peerConnection

function joinCall(receiver) {
    initializeHubConnection()
    setupPeerConnection()

    rtcHub.invoke("JoinCall", receiver)
}

function handleOffer(data) {
    console.log("Processing offer: ", data.value)

    peerConn.setRemoteDescription(JSON.parse(data.value))
    peerConn.createAnswer((answer) => {
        peerConn.setLocalDescription(answer)
        connection.invoke("SendAnswer", username, JSON.stringify(answer))
    }, error => {
        console.log(error)
    })
}

function handleAnswer(data) {
    console.log("Processing answer: ", data.value)
    peerConn.setRemoteDescription(JSON.parse(data.value))
}

function handleCandidate(data) {
    console.log("Processing candidate: ", data.value)
    peerConn.addIceCandidate(JSON.parse(data.value))
}

function initializeHubConnection() {
    rtcHub = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7077/webrtc", {
            accessTokenFactory: () => {
                return localStorage.getItem("BearerToken");
            }
        })
        .build()

    rtcHub.on("ReceiveAnswer", (data) => handleAnswer(data))
    rtcHub.on("ReceiveCandidate", (data) => handleCandidate(data))

    rtcHub.start().then(() => console.log("Connection established."))
        .catch(err => console.error(err));
}

function setupPeerConnection() {
    document.getElementById("video-call-div")
        .style.display = "inline"

    navigator.getUserMedia({
        video: {
            frameRate: 24,
            width: {
                min: 480, ideal: 720, max: 1280
            },
            aspectRatio: 1.33333
        },
        audio: true
    }, (stream) => {
        localStream = stream
        document.getElementById("local-video").srcObject = localStream

        peerConn = new RTCPeerConnection(configuration)
        peerConn.addStream(localStream)

        peerConn.onaddstream = (e) => {
            document.getElementById("remote-video")
                .srcObject = e.stream
        }
        peerConn.onicecandidate = ((e) => {
            if (e.candidate == null)
                return
            username = document.getElementById("username-input").value
            rtcHub.invoke("StoreCandidate", JSON.stringify(e.candidate))
        })

        sendOffer()
    }, (error) => {
        console.log(error)
    })
}

function sendOffer() {
    peerConn.createOffer((offer) => {
        rtcHub.invoke("StoreOffer", JSON.stringify(offer))
        peerConn.setLocalDescription(offer)
    }, (error) => {
        console.log(error)
    })
}

// Call settings
let isAudio = true
function muteAudio() {
    isAudio = !isAudio
    localStream.getAudioTracks()[0].enabled = isAudio
}

let isVideo = true
function muteVideo() {
    isVideo = !isVideo
    localStream.getVideoTracks()[0].enabled = isVideo
}