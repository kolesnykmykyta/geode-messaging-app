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
async function joinGroupCall(group) {
    console.log("Joining group call: ", group)
    await joinCall("JoinCall", group)
}

async function joinPrivateCall(receiver) {
    console.log("Joining private call: ", receiver)
    await joinCall("JoinPrivateCall", receiver)
}

async function joinCall(hubMethod, callId) {
    await initializeHubConnection()
    setupLocalStream()
    rtcHub.invoke(hubMethod, callId)
}

// SignalR messages handlers
function initiateOffer(peerName) {
    newConnection = createPeerConnection(peerName)
    newConnection.createOffer((offer) => {
        console.log("Creating offer for: ", peerName)
        newConnection.setLocalDescription(offer)
        rtcHub.invoke("SendOffer", peerName, JSON.stringify(offer))
    }, (error) => {
        console.log(error)
    })
}

function handleOffer(sender, data) {
    console.log("Handling offer from: ", sender)
    newConnection = createPeerConnection(sender)
    newConnection.setRemoteDescription(JSON.parse(data))
    newConnection.createAnswer((answer) => {
        console.log("Creating answer for: ", sender)
        newConnection.setLocalDescription(answer)
        rtcHub.invoke("SendAnswer", sender, JSON.stringify(answer))
    }, error => {
        console.log(error)
    })
}

function handleAnswer(sender, data) {
    console.log("Handling answer from: ", sender)
    let targetedConnection = findConnectionByPeerName(sender)
    if (targetedConnection) {
        targetedConnection.setRemoteDescription(JSON.parse(data))
    }
}

function handleCandidate(sender, data) {
    console.log("Handling candidate from: ", sender)
    let targetedConnection = findConnectionByPeerName(sender)
    if (targetedConnection) {
        targetedConnection.addIceCandidate(JSON.parse(data))
    }
}
function removePeerVideo(peerVideoId) {
    console.log("Deleting video with id: ", peerVideoId)
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
    console.log("Creating new connection with: ", peer)
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
    if (localStream) {
        let localAudio = localStream.getAudioTracks()[0]
        if (localAudio) {
            isAudio = !isAudio
            localAudio.enabled = isAudio
        }
    }
}

function getAudioStatus() {
    return isAudio
}

function changeVideoStatus() {
    if (localStream) {
        let localVideo = localStream.getVideoTracks()[0]
        if (localVideo) {
            isVideo = !isVideo
            localVideo.enabled = isVideo
        }
    }
}

function getVideoStatus() {
    return isVideo
}

// Helpers
function findConnectionByPeerName(peerName) {
    let targetedConnection
    peerConnections.some(function (obj) {
        if (obj.peerUsername == peerName) {
            targetedConnection = obj;
            return true;
        }
    });

    return targetedConnection
}
