var createRoomBtn = document.getElementById('create-room-btn');
var createRoomModal = document.getElementById('create-room-modal');
var removeRoomBtn = document.getElementById('remove-room-btn');
var removeRoomModal = document.getElementById('remove-room-modal');

if (createRoomBtn) {
    createRoomBtn.addEventListener('click', function () {

        createRoomModal.classList.add('active');
    })
}

function closeModal() {
    createRoomModal.classList.remove('active');
}

if (removeRoomBtn) {
    removeRoomBtn.addEventListener('click', function () {

        removeRoomModal.classList.add('active');
    })
}

function closeRemoveModal() {
    removeRoomModal.classList.remove('active');
}