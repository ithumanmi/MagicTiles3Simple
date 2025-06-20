

## 🚀 Hướng dẫn cài đặt và chạy project

1.  **Clone Repository:**
    ```bash
    git clone <your-repository-link>
    ```
2.  **Mở Project trong Unity:**
    *   Sử dụng Unity Hub để mở project.
    *   Phiên bản Unity được đề xuất: **2021.3.45f1** hoặc mới hơn.
    *   Unity sẽ mất một lúc để import project và các package lần đầu.
3.  **Chạy Game:**
    *   Mở scene chính tại: `Assets/Scenes/main.unity`.
    *   Nhấn nút **Play** trong Unity Editor để bắt đầu chơi.

---

## 🛠️ Lựa chọn thiết kế (Design Choices)

Dự án được xây dựng dựa trên các mẫu thiết kế phổ biến và hiệu quả trong phát triển game bằng Unity.

*   **Singleton Pattern (`GameManager`, `TileSpawner`):**
    *   Sử dụng Singleton để đảm bảo chỉ có một thực thể của các manager chính tồn tại và có thể được truy cập dễ dàng từ bất kỳ đâu trong project (ví dụ: `GameManager.Instance`). Điều này giúp quản lý trạng thái game (điểm số, combo, win/loss) một cách tập trung.

*   **Object Pooling (`TilePooler`):**
    *   Thay vì liên tục `Instantiate` và `Destroy` các tile, project sử dụng một hệ thống "pool" để tái sử dụng chúng. Việc này giúp giảm đáng kể lượng rác (garbage) sinh ra, tránh hiện tượng giật lag (stutter) do Garbage Collection, điều tối quan trọng đối với một game đòi hỏi sự chính xác về thời gian như rhythm game.

*   **Độ khó động (Dynamic Difficulty):**
    *   Game có thể tự động tăng độ khó theo thời gian bằng cách giảm `beatInterval` (tốc độ ra tile) trong `GameManager`.
    *   Cung cấp tùy chọn `IsEasy` để tắt tính năng này, cho phép người chơi trải nghiệm ở một tốc độ không đổi.

*   **Kiến trúc dựa trên Component (Component-Based Architecture):**
    *   Tuân thủ triệt để kiến trúc của Unity, mỗi đối tượng trong game được cấu thành từ các component độc lập (ví dụ: `TileController`, `SpriteRenderer`, `Collider2D`). Logic game được chia nhỏ vào các script khác nhau, giúp dễ quản lý và tái sử dụng.

*   **Quản lý tập trung:**
    *   Các thành phần cốt lõi như `GameManager`, `UIManager`, `TileSpawner`, `MusicManager` được thiết kế để quản lý các khía cạnh riêng biệt của game, giúp code có tổ chức và dễ bảo trì.

---

## 📦 Assets và thư viện bên ngoài

Dự án có sử dụng các assets từ bên thứ ba:

1.  **Epic Toon FX:**
    *   **Nguồn:** Unity Asset Store.
    *   **Sử dụng:** Cung cấp các hiệu ứng hình ảnh (visual effects) như cháy nổ, lấp lánh để tăng tính thẩm mỹ cho game.

2.  **Demigiant DOTween:**
    *   **Nguồn:** Unity Asset Store / [dotween.demigiant.com](http://dotween.demigiant.com/).
    *   **Sử dụng:** Một thư viện animation mạnh mẽ, được dùng để tạo các hiệu ứng chuyển động mượt mà cho UI và các đối tượng trong game (ví dụ: animation của tile khi được nhấn).

3.  **TextMesh Pro:**
    *   **Nguồn:** Unity Package Manager (tích hợp sẵn).
    *   **Sử dụng:** Dùng để hiển thị văn bản (điểm số, combo) với chất lượng cao và khả năng tùy biến mạnh mẽ.

---

## SafeArea Helper cho Unity

Để đảm bảo UI hiển thị đúng trên các thiết bị có notch, viền cong hoặc tỷ lệ màn hình đặc biệt, dự án sử dụng **SafeArea Helper**:

- **Mục đích:**
  - Tự động căn chỉnh UI vào vùng an toàn (safe area) trên mọi thiết bị di động.
  - Tránh việc UI bị che khuất bởi notch, camera, hoặc các cạnh cong của màn hình.

- **Tham khảo:**
  - [GitHub - Unity SafeArea (by mob-sakai)](https://github.com/mob-sakai/Unity-SafeArea)
  - [Asset Store - Safe Area Helper](https://assetstore.unity.com/packages/tools/gui/safe-area-helper-144645)

- **Hướng dẫn sử dụng:**
  1. Import package SafeArea Helper vào project.
  2. Thêm component `SafeArea` vào Canvas hoặc RectTransform chứa UI cần bảo vệ.
  3. UI sẽ tự động điều chỉnh theo vùng an toàn của thiết bị khi chạy trên iOS/Android.

> **Lưu ý:** Nếu không dùng package này, UI có thể bị che khuất hoặc hiển thị sai lệch trên các thiết bị có notch hoặc tỷ lệ màn hình đặc biệt.

----
### DRY & Reusability
Các đoạn code lặp lại (ví dụ: scale tile theo màn hình, reset trạng thái tile, di chuyển tile, v.v.) được gom vào các class cha (ví dụ: BaseTileController), giúp giảm trùng lặp, dễ bảo trì và mở rộng.
Sử dụng Pooling cho tile để tối ưu hiệu năng và giảm GC.


## Third-party Integration
Sử dụng các package phổ biến như DOTween (cho animation), TextMeshPro (cho UI text), SafeArea Helper (cho UI đa thiết bị), giúp tăng tốc phát triển và đảm bảo chất lượng.
Các package này đều được quản lý qua Package Manager hoặc Plugins folder, dễ update và thay thế.

## Maintainability & Scalability
Logic game (GameManager) tách biệt với UI, âm thanh, và các hệ thống khác, giúp dễ mở rộng tính năng mới mà không ảnh hưởng đến phần còn lại.
Sử dụng Singleton cho các manager chính, đảm bảo truy cập toàn cục nhưng vẫn kiểm soát được vòng đời.

## Mobile-first & Responsive UI
UI được thiết kế để tự động căn chỉnh theo Safe Area, đảm bảo hiển thị tốt trên mọi thiết bị di động, kể cả máy có notch hoặc tỷ lệ màn hình đặc biệt.
