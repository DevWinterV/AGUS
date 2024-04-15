$(document).ready(function () {
    $('button.mode-switch').click(function () {
        $('body').toggleClass('dark');
    });

    $(".btn-close-right").click(function () {
        $(".right-side").removeClass("show");
        $(".expand-btn").addClass("show");
    });

    $(".expand-btn").click(function () {
        $(".right-side").addClass("show");
        $(this).removeClass("show");
    });
});
document.addEventListener("keydown", function (e) {
    if (e.key === "F12" || (e.ctrlKey && e.shiftKey && e.key === "I")) {
        e.preventDefault();
    }
});

document.addEventListener("contextmenu", function (e) {
    e.preventDefault();
});


// Function to play the notification sound
function playNotificationSound() {
    var notificationSound = document.getElementById("notificationSound");

    // Check if the audio element is already playing, pause and reset it
    if (!notificationSound.paused) {
        notificationSound.pause();
        notificationSound.currentTime = 0;
    }

    // Ensure that the audio is loaded before playing
    notificationSound.addEventListener('canplaythrough', function () {
        notificationSound.play();
    });

    // In case 'canplaythrough' event doesn't fire, try to play the audio
    notificationSound.play().catch(function (error) {
        // Handle the error, if any
        console.error("Error playing audio:", error);
    });
}
document.addEventListener("DOMContentLoaded", function () {
    var emojiSelect = document.getElementById("emojiSelect");

    // List of emojis
    var emojis = ["❤️", "👍", "😊", "😂", "🔥", "🎉", "🚀", "🌈", "🍕", "🍦", "🎈", "🎁", "🍔", "🍟", "🌍", "🚗", "🏆", "⌛", "💡", "📱", "🎵", "🏖️", "⚽", "🎮", "📚", "🍎", "🍭", "☕", "🍻", "🌺", "🌙", "💤", "💪", "💔", "👋", "🙌", "🍇", "🌟", "👑", "💎", "🎀", "🚢", "🚁", "🚲", "🍓", "🍒", "🍍", "🍌", "🍊", "🍋", "🍉", "🍈", "🍏", "🥑", "🍆", "🍅", "🌽", "🍠", "🍤", "🍥", "🍣", "🍢", "🍡", "🍦", "🍧", "🍨", "🍩", "🍪", "🎂", "🍰", "🧁", "🍫", "🍬", "🍭", "🍮", "🍯", "🍩", "☕", "🍵", "🍶", "🍷", "🍸", "🍹", "🍺", "🍻", "🥂", "🥃", "🥤"];

    // Shuffle emojis randomly
    emojis = shuffleArray(emojis);

    emojis.forEach(function (emoji) {
        var option = document.createElement("option");
        option.value = emoji;
        option.text = emoji;
        emojiSelect.add(option);
    });
    // Add click event to insert selected emoji into the text input
    emojiSelect.addEventListener("change", function () {
        var selectedEmoji = emojiSelect.value;
        messageInput.value += selectedEmoji;
    });
});

// Shuffle array function
function shuffleArray(array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}