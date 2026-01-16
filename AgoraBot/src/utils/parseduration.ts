export function parseDuration(input: string): number | null {
    const match = input.match(/^(\d+)\s*(s|m|h|d)$/i);
    if (!match) return null;

    const value = parseInt(match[1]);
    const unit = match[2].toLowerCase();

    switch (unit) {
        case "s":
            return value * 1000;            // seconds
        case "m":
            return value * 60 * 1000;       // minutes
        case "h":
            return value * 60 * 60 * 1000;  // hours
        case "d":
            return value * 24 * 60 * 60 * 1000; // days
        default:
            return null;
    }
}