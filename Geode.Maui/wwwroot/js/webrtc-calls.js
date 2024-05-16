let rtcHub
let localStream
let peerConnection
let receiver

async function joinCall(username) {
    receiver = username
    await initializeHubConnection()
    setupPeerConnection()
}

function handleOffer(data) {
    console.log("Processing offer: ", data)
    peerConnection.setRemoteDescription(JSON.parse(data))
    peerConnection.createAnswer((answer) => {
        console.log("Sending answer")
        peerConnection.setLocalDescription(answer)
        rtcHub.invoke("SendAnswer", receiver, JSON.stringify(answer))
    }, error => {
        console.log(error)
    })
}

function handleAnswer(data) {
    console.log("Processing answer: ", data)
    peerConnection.setRemoteDescription(JSON.parse(data))
}

function handleCandidate(data) {
    console.log("Processing candidate: ", data)
    peerConnection.addIceCandidate(JSON.parse(data))
}

function initiateOffer(username) {
    console.log("Creating offer")
    peerConnection.createOffer((offer) => {
        peerConnection.setLocalDescription(offer)
        rtcHub.invoke("SendOffer", username, JSON.stringify(offer))
    }, (error) => {
        console.log(error)
    })
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

    await rtcHub.start().then(() => console.log("Connection established."))
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
        const configuration = {
            iceServers: [
                {
                    "urls": ["stun:stun.l.google.com:19302",
                        "stun:stun1.l.google.com:19302",
                        "stun:stun2.l.google.com:19302"]
                }
            ]
        }

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
