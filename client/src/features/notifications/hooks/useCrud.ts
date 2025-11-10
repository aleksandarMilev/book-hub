import api from '@/features/notifications/api/api';
import type { NotificationType } from '@/features/notifications/types/notification';
import baseCrudBuilder from '@/shared/hooks/useCrud/baseCrudBuilder';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export const { useAll, useRemove, usePatch, useDetails } = baseCrudBuilder<
  PaginatedResult<NotificationType>,
  Notification,
  null
>()
  .with()
  .api(api)
  .and()
  .resource('Notification')
  .and()
  .errors(errors.notification)
  .create();
