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
let peerConnections = []

// Local stream settings
let isAudio = true
let isVideo = true

// Main functions to start the call
async function joinCall(group) {
    await initializeHubConnection()
    setupLocalStream()
    rtcHub.invoke("JoinCall", group)
}

async function joinPrivateCall(receiver) {
    await initializeHubConnection()
    setupLocalStream()
    rtcHub.invoke("JoinPrivateCall", receiver)
}

// SignalR messages handlers
function initiateOffer(username) {
    newConnection = createPeerConnection(username)
    newConnection.createOffer((offer) => {
        newConnection.setLocalDescription(offer)
        rtcHub.invoke("SendOffer", username, JSON.stringify(offer))
    }, (error) => {
        console.log(error)
    })
}

function handleOffer(sender, data) {
    newConnection = createPeerConnection(sender)
    newConnection.setRemoteDescription(JSON.parse(data))
    newConnection.createAnswer((answer) => {
        newConnection.setLocalDescription(answer)
        rtcHub.invoke("SendAnswer", sender, JSON.stringify(answer))
    }, error => {
        console.log(error)
    })
}

function handleAnswer(sender, data) {
    let targetedConnection
    peerConnections.some(function (obj) {
        if (obj.peerUsername == sender) {
            targetedConnection = obj;
            return true;
        }
    });

    targetedConnection.setRemoteDescription(JSON.parse(data))
}

function handleCandidate(sender, data) {
    let targetedConnection
    peerConnections.some(function (obj) {
        if (obj.peerUsername == sender) {
            targetedConnection = obj;
            return true;
        }
    });

    targetedConnection.addIceCandidate(JSON.parse(data))
}
function removePeerVideo(peerVideoId) {
    let videoToRemove = document.getElementById(peerVideoId)
    if (videoToRemove) {
        let parent = videoToRemove.parentNode
        parent.removeChild(videoToRemove)
    }
}

// Setups
function setupLocalStream() {
    isVideo = true
    isAudio = true
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
    }, error => console.log(error))
}
async function initializeHubConnection() {
    accessToken = localStorage.getItem("BearerToken")
    rtcHub = new signalR.HubConnectionBuilder()
        .withUrl("https://geode-api.azurewebsites.net/webrtc", {
            accessTokenFactory: () => accessToken
        })
        .build()

    rtcHub.on("InitiateOffer", (username) => initiateOffer(username))
    rtcHub.on("ReceiveOffer", (sender, data) => handleOffer(sender, data))
    rtcHub.on("ReceiveAnswer", (sender, data) => handleAnswer(sender, data))
    rtcHub.on("ReceiveCandidate", (sender, data) => handleCandidate(sender, data))
    rtcHub.on("RemovePeerVideo", (username) => removePeerVideo(username))

    await rtcHub.start()
        .then(() => console.log("Connection established."))
        .catch(err => console.error(err));
}

function createPeerConnection(peer) {
    let newConnection = new RTCPeerConnection(configuration)
    newConnection.peerUsername = peer
    newConnection.addStream(localStream)

    newConnection.onaddstream = (e) => {
        console.log("Creating new video")
        newVideo = document.createElement("video")
        newVideo.id = newConnection.peerUsername
        newVideo.srcObject = e.stream
        newVideo.autoplay = true
        newVideo.controls = false
        newVideo.muted = false
        document.getElementById("videos-div").appendChild(newVideo)
    }
    newConnection.onicecandidate = ((e) => {
        if (e.candidate == null)
            return
        rtcHub.invoke("ProcessCandidate", newConnection.peerUsername, JSON.stringify(e.candidate))
    })

    peerConnections.push(newConnection)
    return newConnection
}

// Cleanups
function disposeCleanup() {
    stopRtcConnections()
    stopHubConnection()
    stopMediaTracks()
}

function stopHubConnection() {
    rtcHub.invoke("CleanUserData")
        .then(() => rtcHub.stop())
        .then(() => console.log("Connection closed."))
        .catch(err => console.error(err));
}

function stopRtcConnections() {
    if (peerConnections) {
        peerConnections.forEach(conn => {
            if (conn) {
                rtcHub.invoke("RemoveUserVideo", conn.peerUsername)
                conn.close();
                conn = null;
            }
        })

        peerConnections = []
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
function changeAudioStatus() {
    isAudio = !isAudio
    localStream.getAudioTracks()[0].enabled = isAudio
}

function getAudioStatus() {
    return isAudio
}

function changeVideoStatus() {
    isVideo = !isVideo
    localStream.getVideoTracks()[0].enabled = isVideo
}

function getVideoStatus() {
    return isVideo
}
