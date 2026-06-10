# SLA Scoring System — Specification

> نسخه: ۱.۰  
> وضعیت: نهایی  
> این داکیومنت جایگزین تمام پیش‌نویس‌های قبلی SLA است.

---

## ۱. تعاریف پایه

### ۱.۱ اصطلاحات

| اصطلاح | تعریف |
|--------|-------|
| **Carrier** | همان Courier در سیستم — شرکت حمل‌ونقل |
| **SLA Score** | امتیاز کلی عملکرد Carrier (۰ تا ۱) |
| **KPI** | شاخص عملکرد کلیدی — پنج شاخص تعریف شده |
| **Snapshot** | محاسبه و ذخیره SLA در یک لحظه زمانی |
| **Rolling Window** | بازه ۳۰ روز گذشته — برای Assignment Engine |
| **Fixed Period** | ماه/هفته/روز ثابت — برای گزارش داشبورد |

### ۱.۲ پنج KPI اصلی

| # | KPI | واحد | منبع داده |
|---|-----|------|-----------|
| ۱ | SuccessRate | درصد | FinalizedDelivery / TotalAssigned |
| ۲ | OnTimeDelivery | درصد | DeliveredAt ≤ PromisedDeliveryAt / TotalDelivered |
| ۳ | ReturnRate | درصد | Returned / TotalAssigned |
| ۴ | AvgDeliveryTime | ساعت | میانگین (DeliveredAt − AssignedAt) |
| ۵ | AvgCostPerParcel | تومان | میانگین (PickupCost + DistributionCost) |

> **تفاوت کلیدی SuccessRate و OnTimeDelivery:**  
> SuccessRate = آیا تحویل داده شد؟ (بدون در نظر گرفتن زمان)  
> OnTimeDelivery = آیا در موعد مقرر تحویل داده شد؟

---

## ۲. بازه زمانی محاسبه

دو بازه موازی، دو هدف متفاوت:

```
Rolling 30 روز  →  Assignment Engine (تصمیم‌گیری real-time)
Fixed Period    →  Dashboard و گزارش (مقایسه دوره‌ای)
```

### ۲.۱ Rolling 30 روز

- همیشه از امروز به عقب ۳۰ روز
- برای الگوریتم assignment استفاده می‌شود
- هر ۶ ساعت یک‌بار snapshot گرفته می‌شود

### ۲.۲ Fixed Period (Dashboard)

- ادمین بازه انتخاب می‌کند: روزانه / هفتگی / ماهانه
- برای Trend Chart و مقایسه تاریخی
- snapshot روزانه ذخیره می‌شود

---

## ۳. نرمال‌سازی KPIها (۰ تا ۱)

### SuccessRate

```
S = SuccessRate / 100

مثال: SuccessRate = 97%  →  S = 0.97
```

### OnTimeDelivery

```
O = OnTimeDelivery / 100

مثال: OnTimeDelivery = 88%  →  O = 0.88
```

### ReturnRate

```
R = 1 - (ReturnRate / 100)

مثال: ReturnRate = 5%  →  R = 1 - 0.05 = 0.95
```

### AvgDeliveryTime → DeliveryTimeScore

```
D = min(1.0,  SlaHours / AvgDeliveryHours)

منطق: هرچه AvgDelivery از SLA بیشتر باشد، score کمتر
cap at 1.0: اگر سریع‌تر از SLA → score = 1.0

مثال (SlaHours = 24):
  AvgDelivery = 20h  →  D = min(1.0, 24/20) = 1.0
  AvgDelivery = 24h  →  D = min(1.0, 24/24) = 1.0
  AvgDelivery = 30h  →  D = min(1.0, 24/30) = 0.80
  AvgDelivery = 48h  →  D = min(1.0, 24/48) = 0.50
```

### AvgCostPerParcel → CostEfficiencyScore

```
C = min(1.0,  BenchmarkCost / AvgCostPerParcel)

BenchmarkCost: عدد ثابت تعریف‌شده توسط Super Admin
منطق: هرچه هزینه از benchmark بیشتر باشد، score کمتر

مثال (BenchmarkCost = 100,000):
  AvgCost = 80,000  →  C = min(1.0, 100k/80k)  = 1.0
  AvgCost = 100,000 →  C = min(1.0, 100k/100k) = 1.0
  AvgCost = 120,000 →  C = min(1.0, 100k/120k) = 0.83
  AvgCost = 200,000 →  C = min(1.0, 100k/200k) = 0.50
```

---

## ۴. فرمول SLA Score

### ۴.۱ حالت عادی (همه KPIها موجود)

```
SLA Score = (S × w1) + (O × w2) + (R × w3) + (D × w4) + (C × w5)

وزن‌های پیش‌فرض:
  w1 = 0.30  (SuccessRate)
  w2 = 0.30  (OnTimeDelivery)
  w3 = 0.20  (ReturnRate)
  w4 = 0.10  (DeliveryTimeScore)
  w5 = 0.10  (CostEfficiencyScore)
  ─────────
  Σ  = 1.00
```

### ۴.۲ حالت KPI ناقص (re-weighting)

اگر یک یا چند KPI قابل محاسبه نباشد، وزن‌های باقیمانده نرمال می‌شوند:

```
available_weights = مجموع وزن KPIهای موجود
new_weight_i = w_i / available_weights

مثال: DeliveryTimeScore (w4=0.10) موجود نیست:
  available_weights = 0.30 + 0.30 + 0.20 + 0.10 = 0.90
  new_w1 = 0.30 / 0.90 = 0.333
  new_w2 = 0.30 / 0.90 = 0.333
  new_w3 = 0.20 / 0.90 = 0.222
  new_w5 = 0.10 / 0.90 = 0.111

نتیجه: Score همچنان بین 0 تا 1 باقی می‌ماند
       HasMissingKpi = true → در UI هشدار نمایش داده می‌شود
```

### ۴.۳ رنگ‌بندی وضعیت

| رنگ | بازه | توضیح |
|-----|------|-------|
| سبز | ≥ 0.85 | عملکرد عالی |
| زرد | 0.75 – 0.84 | عملکرد متوسط |
| قرمز | < 0.75 | عملکرد ضعیف + هشدار |

---

## ۵. آستانه‌های هشدار per-Carrier

هر Carrier آستانه‌های اختصاصی دارد. اگر تعریف نشده → Global fallback:

| KPI | آستانه هشدار Global | توضیح |
|-----|---------------------|-------|
| SuccessRate | < 95% | قرمز |
| OnTimeDelivery | < 90% | قرمز |
| ReturnRate | > 5% | قرمز |
| AvgDeliveryTime | > SlaHours × 1.2 | قرمز |
| AvgCostPerParcel | > BenchmarkCost × 1.15 | قرمز |

---

## ۶. تغییر وزن‌ها

- فقط Super Admin می‌تواند وزن‌ها را تغییر دهد
- هر تغییر در `SlaGlobalConfig` با `EffectiveFrom` ذخیره می‌شود
- تغییر وزن روی **snapshot‌های گذشته اثر ندارد**
- هر snapshot به `SlaGlobalConfigId` خود ref می‌دهد
- Audit Log: CreatedByUserId + CreatedAt در هر record

---

## ۷. معماری محاسبه

```
Background Job (هر ۶ ساعت):
  ← KPIهای خام از DB محاسبه می‌شوند
  ← نرمال‌سازی انجام می‌شود
  ← SLA Score محاسبه می‌شود
  ← در CarrierSlaSnapshot ذخیره می‌شود
  ← اگر آستانه رد شد → SlaAlert ایجاد می‌شود

Dashboard:
  ← فقط از CarrierSlaSnapshot می‌خواند
  ← همیشه زیر ۲ ثانیه

Counter‌های ساده (real-time):
  ← TodayAssigned, TodayDelivered
  ← از CourierDailyCapacity (atomic increment)
  ← سبک، بدون محاسبه سنگین
```

---

## ۸. هشدارها

### ۸.۱ فاز ۱ — Dashboard Badge

- هشدار در داشبورد به‌صورت badge/flag نمایش داده می‌شود
- اگر همان AlertType برای همان Carrier در ۶ ساعت گذشته وجود داشت → alert جدید ساخته نمی‌شود (dedup)
- تاریخچه هشدارها در `SlaAlert` نگه داشته می‌شود

### ۸.۲ انواع هشدار

```csharp
SlaAlertType
  SlaScoreLow         = 0  // SLA Score < 0.75
  SuccessRateLow      = 1  // SuccessRate < threshold
  OnTimeLow           = 2  // OnTimeDelivery < threshold
  ReturnRateHigh      = 3  // ReturnRate > threshold
  DeliveryTimeSlow    = 4  // AvgDeliveryTime > SLA × 1.2
  CostHigh            = 5  // AvgCost > Benchmark × 1.15
  MissingKpiData      = 6  // یک یا چند KPI ناقص
```

### ۸.۳ فاز ۲ (آینده)

- Email notification
- Push notification

---

## ۹. DB Schema — جداول جدید و تغییرات

### ۹.۱ جداول جدید

```sql
-- تنظیمات SLA هر Courier
CourierSlaConfig
  CourierSlaConfigId  INT PK
  CourierId           INT FK
  ServiceType         INT         -- 0=LastMile, 1=Express
  SlaHours            INT         -- موعد تحویل (ساعت)
  SuccessRateMin      DECIMAL(5,4) -- آستانه هشدار، مثلاً 0.95
  OnTimeMin           DECIMAL(5,4)
  ReturnRateMax       DECIMAL(5,4) -- آستانه هشدار، مثلاً 0.05
  CreatedByUserId     INT
  ModifiedByUserId    INT
  ModifiedDateTime    DATETIME
  CreatedAt           DATETIME
  IsActive            BIT
  IsDeleted           BIT

-- تنظیمات global، وزن‌ها و benchmark
SlaGlobalConfig
  SlaGlobalConfigId       INT PK
  BenchmarkCostPerParcel  DECIMAL      -- مرجع هزینه (admin تعریف می‌کند)
  WeightSuccessRate       DECIMAL(5,4) -- پیش‌فرض 0.30
  WeightOnTimeDelivery    DECIMAL(5,4) -- پیش‌فرض 0.30
  WeightReturnRate        DECIMAL(5,4) -- پیش‌فرض 0.20
  WeightDeliveryTime      DECIMAL(5,4) -- پیش‌فرض 0.10
  WeightCost              DECIMAL(5,4) -- پیش‌فرض 0.10
  EffectiveFrom           DATETIME     -- از کی اعمال می‌شود
  CreatedByUserId         INT
  CreatedAt               DATETIME
  -- constraint: مجموع وزن‌ها = 1.0

-- snapshot تاریخی محاسبه‌شده
CarrierSlaSnapshot
  SnapshotId              INT PK
  CourierId               INT FK
  SlaGlobalConfigId       INT FK       -- کدام وزن‌ها استفاده شد
  PeriodType              INT          -- 0=Rolling30, 1=Daily, 2=Weekly, 3=Monthly
  PeriodStart             DATE
  PeriodEnd               DATE
  -- KPI خام
  TotalAssigned           INT
  TotalDelivered          INT
  TotalOnTime             INT
  TotalReturned           INT
  AvgDeliveryHours        DECIMAL NULL
  AvgCostPerParcel        DECIMAL NULL
  -- KPI نرمال‌شده
  SuccessRate             DECIMAL(5,4)
  OnTimeDelivery          DECIMAL(5,4)
  ReturnRate              DECIMAL(5,4)
  DeliveryTimeScore       DECIMAL(5,4) NULL
  CostEfficiencyScore     DECIMAL(5,4) NULL
  -- نتیجه
  SlaScore                DECIMAL(5,4)
  HasMissingKpi           BIT
  MissingKpiFlags         INT          -- bitmask: کدام KPIها ناقص بودند
  CreatedAt               DATETIME

-- هزینه هر مرسوله
ParcelCost
  ParcelCostId            INT PK
  ParcelCourierId         INT FK
  PickupCost              DECIMAL
  DistributionCost        DECIMAL
  TotalCost AS (PickupCost + DistributionCost) PERSISTED
  CreatedAt               DATETIME
  CreatedByUserId         INT

-- هشدارهای SLA
SlaAlert
  SlaAlertId              INT PK
  CourierId               INT FK
  SnapshotId              INT FK
  AlertType               INT          -- SlaAlertType enum
  IsRead                  BIT
  CreatedAt               DATETIME

-- ظرفیت روزانه Carrier
CourierDailyCapacity
  CourierId               INT FK
  Date                    DATE
  UsedCapacity            INT DEFAULT 0
  CONSTRAINT UQ_Courier_Date UNIQUE (CourierId, Date)
```

### ۹.۲ فیلدهای اضافه به جداول موجود

```sql
-- به ParcelCourier اضافه می‌شود
AssignedAt          DATETIME       -- لحظه تخصیص به Courier
PromisedDeliveryAt  DATETIME       -- موعد SLA (محاسبه در زمان assign)

-- به ParcelCourierFleet اضافه می‌شود
DeliveredAt         DATETIME NULL  -- لحظه تحویل واقعی (پر می‌شود هنگام FinalizedDelivery)
```

### ۹.۳ نحوه محاسبه PromisedDeliveryAt

```
هنگام assign:
  config = CourierSlaConfig WHERE CourierId = X AND ServiceType = Y AND IsActive = 1
  اگر config نبود → SlaGlobalConfig.DefaultSlaHours (fallback)
  PromisedDeliveryAt = AssignedAt + config.SlaHours (ساعت)
```

---

## ۱۰. Acceptance Criteria نهایی

### SLA Score

- [x] سیستم SLA Score را برای همه Carrierها هر ۶ ساعت محاسبه کند
- [x] SLA Score بر اساس وزن‌های تعریف‌شده باشد
- [x] تغییر وزن‌ها روی snapshot‌های گذشته اثر نگذارد
- [x] اگر KPI ناقص بود → re-weight و هشدار MissingKpiData
- [x] SLA Score در داشبورد با رنگ‌بندی سبز/زرد/قرمز نمایش داده شود
- [x] Drill-down به KPIهای جزئی از SLA Score امکان‌پذیر باشد

### KPIها

- [x] SuccessRate: تحویل موفق / کل تخصیص‌یافته × 100
- [x] OnTimeDelivery: تحویل به‌موقع / کل تحویل‌شده × 100 (نیاز به PromisedDeliveryAt)
- [x] ReturnRate: مرجوعی / کل تخصیص‌یافته × 100
- [x] AvgDeliveryTime: میانگین (DeliveredAt − AssignedAt) برحسب ساعت
- [x] AvgCostPerParcel: میانگین (PickupCost + DistributionCost)

### داشبورد

- [x] فیلتر بر اساس بازه زمانی (روزانه/هفتگی/ماهانه)
- [x] مقایسه حداقل ۳ Carrier روی یک نمودار
- [x] هایلایت افت شدید KPI
- [x] Export به Excel/CSV
- [x] داده‌های تاریخی حداقل ۱۲ ماه نگه‌داری شوند

### Performance

- [x] Dashboard response < 2 ثانیه (از snapshot)
- [x] Background job هر ۶ ساعت
- [x] Counter‌های روزانه real-time (atomic)

---

## ۱۱. سناریوها

### Happy Path

```
Carrier A:
  SuccessRate=97%  → S=0.97
  OnTime=92%       → O=0.92
  ReturnRate=3%    → R=0.97
  AvgDelivery=22h (SLA=24h) → D=1.0
  AvgCost=90k (Benchmark=100k) → C=1.0

SLA = (0.97×0.3)+(0.92×0.3)+(0.97×0.2)+(1.0×0.1)+(1.0×0.1)
    = 0.291 + 0.276 + 0.194 + 0.1 + 0.1 = 0.961  → سبز
```

### Normal Path

```
Carrier B:
  SuccessRate=91%  → S=0.91
  OnTime=85%       → O=0.85
  ReturnRate=7%    → R=0.93
  AvgDelivery=28h  → D=24/28=0.857
  AvgCost=110k     → C=100/110=0.909

SLA = (0.91×0.3)+(0.85×0.3)+(0.93×0.2)+(0.857×0.1)+(0.909×0.1)
    = 0.273 + 0.255 + 0.186 + 0.086 + 0.091 = 0.891  → سبز (نزدیک به زرد)
```

### Failure Path

```
Carrier C:
  SuccessRate=84%  → S=0.84  + Alert SuccessRateLow
  OnTime=75%       → O=0.75
  ReturnRate=12%   → R=0.88  + Alert ReturnRateHigh
  AvgDelivery=35h  → D=24/35=0.686
  AvgCost=125k     → C=100/125=0.80

SLA = (0.84×0.3)+(0.75×0.3)+(0.88×0.2)+(0.686×0.1)+(0.80×0.1)
    = 0.252 + 0.225 + 0.176 + 0.069 + 0.08 = 0.802  → زرد
```

### Edge Case — KPI ناقص

```
Carrier D: AvgCostPerParcel موجود نیست (هزینه ثبت نشده)

available_weights = 0.30+0.30+0.20+0.10 = 0.90
re-weighted:
  w1=0.333, w2=0.333, w3=0.222, w4=0.111

SLA = (S×0.333)+(O×0.333)+(R×0.222)+(D×0.111)
HasMissingKpi = true
MissingKpiFlags = 0b10000 (bit 4 = Cost)
Alert: MissingKpiData
```

---

## ۱۲. موارد باز (فاز بعدی)

- [ ] Email/Push notification برای هشدارها
- [ ] تعریف SlaHours per-Carrier per-ServiceType در UI
- [ ] Drill-down تا سطح راننده
- [ ] Auto-suggest برای BenchmarkCost بر اساس میانگین Carrierها
