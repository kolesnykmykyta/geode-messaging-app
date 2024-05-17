// Configuration for the RTC connection
const configuration = {
    iceServers: [
        {
            "urls": ["stun:stun.l.google.com:19302",
                "stun:stun1.l.google.com:19302",
                "stun:stun2.l.google.com:19302"]
        }
    ]
}

// Common variables
let rtcHub
let localStream
let peerConnection
let receiver

// Main function to start the call
async function joinCall(username) {
    receiver = username
    await initializeHubConnection()
    setupPeerConnection()
}

// SignalR messages handlers

function initiateOffer(username) {
    peerConnection.createOffer((offer) => {
        peerConnection.setLocalDescription(offer)
        rtcHub.invoke("SendOffer", username, JSON.stringify(offer))
    }, (error) => {
        console.log(error)
    })
}

function handleOffer(data) {
    peerConnection.setRemoteDescription(JSON.parse(data))
    peerConnection.createAnswer((answer) => {
        peerConnection.setLocalDescription(answer)
        rtcHub.invoke("SendAnswer", receiver, JSON.stringify(answer))
    }, error => {
        console.log(error)
    })
}

function handleAnswer(data) {
    peerConnection.setRemoteDescription(JSON.parse(data))
}

function handleCandidate(data) {
    peerConnection.addIceCandidate(JSON.parse(data))
}

// RTC and SignalR setups
function setupPeerConnection() {
    navigator.getUserMedia({
        video: {
            frameRate: 24,
            width: {
                min: 240, ideal: 360, max: 480
            },
            aspectRatio: 1.33333
        },
        audio: true
    }, (stream) => {
        localStream = stream
        document.getElementById("local-video").srcObject = localStream

        peerConnection = new RTCPeerConnection(configuration)
        peerConnection.addStream(localStream)

        peerConnection.onaddstream = (e) => {
            document.getElementById("remote-video")
                .srcObject = e.stream
        }
        peerConnection.onicecandidate = ((e) => {
            if (e.candidate == null)
                return
            rtcHub.invoke("ProcessCandidate", receiver, JSON.stringify(e.candidate))
        })

        rtcHub.invoke("JoinCall", receiver)
    }, (error) => console.log(error))
}
async function initializeHubConnection() {
    accessToken = localStorage.getItem("BearerToken")
    rtcHub = new signalR.HubConnectionBuilder()
        .withUrl("https://geode-api-dev.azurewebsites.net/webrtc", {
            accessTokenFactory: () => accessToken
        })
        .build()

    rtcHub.on("InitiateOffer", (username) => initiateOffer(username))
    rtcHub.on("ReceiveAnswer", (data) => handleAnswer(data))
    rtcHub.on("ReceiveCandidate", (data) => handleCandidate(data))
    rtcHub.on("ReceiveOffer", (data) => handleOffer(data))

    await rtcHub.start()
        .then(() => console.log("Connection established."))
        .catch(err => console.error(err));
}

// Cleanups
function stopHubConnection() {
    rtcHub.invoke("CleanUserData")
        .then(() => rtcHub.stop())
        .then(() => console.log("Connection closed."))
        .catch(err => console.error(err));
}

function closeRtcConnection() {
    if (peerConnection) {
        peerConnection.close();
        peerConnection = null;
    }
}

function stopMediaTracks() {
    if (localStream) {
        localStream.getTracks().forEach(track => {
            track.stop();
        });
        localStream = null;
    }
}

// Local media settings
let isAudio = true
function changeAudioStatus() {
    isAudio = !isAudio
    localStream.getAudioTracks()[0].enabled = isAudio
}

function getAudioStatus() {
    return isAudio
}

let isVideo = true
function changeVideoStatus() {
    isVideo = !isVideo
    localStream.getVideoTracks()[0].enabled = isVideo
}

function getVideoStatus() {
    return isVideo
}
