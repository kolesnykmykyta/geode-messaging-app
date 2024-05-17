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
let groupName

// Main function to start the call
async function joinCall(group) {
    groupName = group
    await initializeHubConnection()
    setupLocalStream()
    rtcHub.invoke("JoinCall", groupName)
}

// SignalR messages handlers

function initiateOffer(username) {
    let newConnection = new RTCPeerConnection(configuration)
    newConnection.peerUsername = username
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

    newConnection.createOffer((offer) => {
        console.log("Creating offer")
        newConnection.setLocalDescription(offer)
        rtcHub.invoke("SendOffer", username, JSON.stringify(offer))
    }, (error) => {
        console.log(error)
    })
}

function handleOffer(sender, data) {
    console.log("Handling offer: ", data)
    let newConnection = new RTCPeerConnection(configuration)
    newConnection.peerUsername = sender
    newConnection.addStream(localStream)

    newConnection.onaddstream = (e) => {
        console.log("Creating new video")
        let newVideo = document.createElement("video")
        newVideo.id = newConnection.peerUsername
        newVideo.autoplay = true;
        newVideo.muted = false;
        newVideo.controls = false;
        newVideo.srcObject = e.stream
        document.getElementById("videos-div").appendChild(newVideo)
    }
    newConnection.onicecandidate = ((e) => {
        if (e.candidate == null)
            return
        rtcHub.invoke("ProcessCandidate", newConnection.peerUsername, JSON.stringify(e.candidate))
    })

    newConnection.setRemoteDescription(JSON.parse(data))
    peerConnections.push(newConnection)

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
            console.log("Handling answer: ", data)
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
            console.log("Handling candidate: ", data)
            targetedConnection = obj;
            return true;
        }
    });

    targetedConnection.addIceCandidate(JSON.parse(data))
}

// Setups
function setupLocalStream() {
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
    rtcHub.on("ReceiveAnswer", (sender, data) => handleAnswer(sender, data))
    rtcHub.on("ReceiveCandidate", (sender, data) => handleCandidate(sender, data))
    rtcHub.on("ReceiveOffer", (sender, data) => handleOffer(sender, data))

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
    if (peerConnections) {
        peerConnections.forEach(conn => {
            if (conn) {
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
