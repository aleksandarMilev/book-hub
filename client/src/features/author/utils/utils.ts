export const formatBiography = (bio: string): string => {
  if (!bio) {
    return '';
  }

  if (bio.includes('\n')) {
    return bio;
  }

  const sentences = bio.split(/(?<=[.!?])\s+/);
  const chunks: string[] = [];
  let buffer: string[] = [];

  sentences.forEach((sentence, index) => {
    buffer.push(sentence);

    const currentLength = buffer.join(' ').length;
    const isLast = index === sentences.length - 1;

    if (currentLength > 280 || isLast) {
      chunks.push(buffer.join(' '));
      buffer = [];
    }
  });

  return chunks.join('\n\n');
};
