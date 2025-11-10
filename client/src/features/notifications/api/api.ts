import type { NotificationType } from '@/features/notifications/types/notification';
import baseApi from '@/shared/api/baseApiBuilder';
import { httpAdminClient, httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export default baseApi<PaginatedResult<NotificationType>, NotificationType, null>()
  .with()
  .getClient(httpClient.get)
  .and()
  .deleteClient(httpAdminClient.delete)
  .and()
  .patchClient(httpAdminClient.patch)
  .and()
  .routes({
    all: routes.notification,
    lastThree: routes.lastThreeNotifications,
    markAsRead: routes.notification,
  })
  .and()
  .errors(errors.notification)
  .create();
