import i18next, { type TFunction } from 'i18next';

import {
  createPasswordSchema,
  PASSWORD_MAX_LENGTH,
  PASSWORD_MIN_LENGTH,
} from '@/features/identity/validation/passwordSchema';
import bgIdentity from '@/shared/i18n/locales/bg/identity.json';
import enIdentity from '@/shared/i18n/locales/en/identity.json';

const REQUIRED_MESSAGE = 'Password is required';

let t: TFunction<'identity'>;

beforeAll(async () => {
  const i18n = i18next.createInstance();
  await i18n.init({
    lng: 'en',
    ns: ['identity'],
    resources: { en: { identity: enIdentity } },
  });

  t = i18n.getFixedT('en', 'identity');
});

const firstError = (password: string) => {
  try {
    createPasswordSchema(t, REQUIRED_MESSAGE).validateSync(password);

    return null;
  } catch (error) {
    return error instanceof Error ? error.message : String(error);
  }
};

describe('createPasswordSchema', () => {
  it('accepts a password that meets every rule', () => {
    expect(firstError('Valid123')).toBeNull();
  });

  it('accepts Cyrillic letters as lowercase and uppercase, like the server', () => {
    expect(firstError('Парола123')).toBeNull();
  });

  it('rejects an empty password with the caller-supplied required message', () => {
    expect(firstError('')).toBe(REQUIRED_MESSAGE);
  });

  it('rejects a password shorter than the minimum length', () => {
    expect(firstError('Short1a')).toBe(
      `Password must be at least ${PASSWORD_MIN_LENGTH} characters long`,
    );
  });

  it('rejects a password longer than the maximum length', () => {
    const tooLong = `Aa1${'x'.repeat(PASSWORD_MAX_LENGTH)}`;

    expect(firstError(tooLong)).toBe(
      `Password must be at most ${PASSWORD_MAX_LENGTH} characters long`,
    );
  });

  it('rejects a password without a digit', () => {
    expect(firstError('NoDigitsHere')).toBe('Password must contain at least one digit');
  });

  it('rejects a password without a lowercase letter', () => {
    expect(firstError('NOLOWER123')).toBe('Password must contain at least one lowercase letter');
  });

  it('rejects a password without an uppercase letter', () => {
    expect(firstError('noupper123')).toBe('Password must contain at least one uppercase letter');
  });

  it('does not require a non-alphanumeric character', () => {
    expect(firstError('Abcdefg1')).toBeNull();
  });

  it('has a Bulgarian translation for every rule message', () => {
    expect(Object.keys(bgIdentity.password.validation).sort()).toEqual(
      Object.keys(enIdentity.password.validation).sort(),
    );
  });
});
