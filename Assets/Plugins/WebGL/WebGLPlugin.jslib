mergeInto(LibraryManager.library, {
    GetJWTPayload: function() {
        var payload = window.jwtPayload ? JSON.stringify(window.jwtPayload) : null;
        var length = lengthBytesUTF8(payload) + 1;
        var stringPointer = _malloc(length);
        stringToUTF8(payload, stringPointer, length);
        return stringPointer;
    }
});
