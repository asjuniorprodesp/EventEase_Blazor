window.eventEaseSession = {
  get: function (key) {
    try {
      const raw = sessionStorage.getItem(key);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  },
  set: function (key, value) {
    try {
      sessionStorage.setItem(key, JSON.stringify(value));
    } catch {
      // ignore storage quota or blocked storage failures
    }
  },
  remove: function (key) {
    try {
      sessionStorage.removeItem(key);
    } catch {
      // ignore storage failures
    }
  }
};
