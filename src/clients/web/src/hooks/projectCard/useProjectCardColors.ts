import { useMemo } from 'react';

export const useProjectCardColors = (projectColor: string) => {
  return useMemo(() => {
    const colorRgb = hexToRgb(projectColor);
    const darkerShade = darkerColor(colorRgb);
    const backgroundColor = `rgb(${colorRgb[0]}, ${colorRgb[1]}, ${colorRgb[2]})`;
    const luminance = calculateLuminance(colorRgb);
    const isDark = luminance < 0.3;
    const borderColor = isDark 
      ? `rgb(${lighterColor(colorRgb)[0]}, ${lighterColor(colorRgb)[1]}, ${lighterColor(colorRgb)[2]})` 
      : `rgb(${darkerShade[0]}, ${darkerShade[1]}, ${darkerShade[2]})`;
    const color = isDark ? 'white' : borderColor;

    return { backgroundColor, borderColor, color };
  }, [projectColor]);
};

const hexToRgb = (hex: string): number[] => {
    const hexRgb: string = hex.replace(/^#?([a-f\d])([a-f\d])([a-f\d])$/i,
        (_, r, g, b) => '#' + r + r + g + g + b + b);

    return hexRgb
        .substring(1).match(/.{2}/g)!
        .map(x => parseInt(x, 16));
}

const darkerColor = (rbg: number[]): number[] => {
    return rbg.map(x => x * 0.6);
}

const lighterColor = (rgb: number[]): number[] => {
    return rgb.map(x => Math.min(255, x + 100));
}

const calculateLuminance = (rgb: number[]): number => {
    const [r, g, b] = rgb.map(x => x / 255);
    const a = [r, g, b].map(v => v <= 0.03928 ? v / 12.92 : Math.pow((v + 0.055) / 1.055, 2.4));
    return 0.2126 * a[0] + 0.7152 * a[1] + 0.0722 * a[2];
}
