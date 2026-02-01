export const toParagraphs = (text: string): string[] => {
  const normalized = text.replace(/\r\n/g, '\n').trim();

  if (/\n\s*\n/.test(normalized)) {
    return normalized
      .split(/\n\s*\n+/)
      .map((p) => p.trim())
      .filter(Boolean);
  }

  const sentences = normalized.split(/(?<=[.!?])\s+(?![a-z]\.)/i).filter(Boolean);

  const ChunkSize = 2;
  const result: string[] = [];

  for (let i = 0; i < sentences.length; i += ChunkSize) {
    result.push(
      sentences
        .slice(i, i + ChunkSize)
        .join(' ')
        .trim(),
    );
  }

  return result;
};

