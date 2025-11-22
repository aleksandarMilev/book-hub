import { useEffect } from 'react';

import { sleep } from '@/shared/lib/utils/utils.js';

export const useSleep = (ms: number = 2_000) => {
  useEffect(() => {
    const wait = async () => {
      await sleep(ms);
    };

    wait();
  }, [ms]);
};
